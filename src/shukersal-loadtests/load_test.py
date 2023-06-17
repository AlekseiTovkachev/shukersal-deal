import requests
import threading
import random
import urllib3
import time

urllib3.disable_warnings(urllib3.exceptions.InsecureRequestWarning)
testkey = random.randint(0, 99999999999999)
test_size = 100
server_url = "https://localhost:7258"

count = 0
global tokens
tokens = dict()
num_of_users = test_size

def register_user(username):
    global count
    url = server_url + "/api/auth/register"
    data = {
        "username": username,
        "password": "string"
    }
    response = requests.post(url, json=data, verify=False)
    if response.status_code < 300:
        count += 1

def login(username):
    global tokens
    url = server_url + "/api/auth/login"
    data = {
        "username": username,
        "password": "string"
    }
    response = requests.post(url, json=data, verify=False)
    if response.status_code == 200:
        global token
        token = response.json()["token"]
        tokens[username] = token
        global user_id
        user_id = response.json()["member"]["id"]
    return response

def create_store(token):
    url = server_url + "/api/stores"
    headers = {
        "Authorization": f"Bearer {token}"
    }
    data = {
        "Name": "store" + testkey.__str__(),
        "Description": testkey.__str__()
    }
    response = requests.post(url, json=data, headers=headers, verify=False)
    if response.status_code < 300:
        pass
    else:
        print(f"Failed to create store: {response.text}")
    return response

def add_product(token, store_id):
    global count
    url = f"{server_url}/api/stores/{store_id}/products"
    headers = {
        "Authorization": f"Bearer {token}"
    }
    data = {
        "name": "string",
        "description": "string",
        "price": 0,
        "unitsInStock": 2147483647,
        "isListed": True,
        "categoryId": 0
    }
    response = requests.post(url, json=data, headers=headers, verify=False)
    if response.status_code < 300:
        count += 1
    else:
        print(f"Failed to create store: {response.text}")
# Number of users to register


# Generate unique usernames
usernames = [f"user{testkey + i}" for i in range(num_of_users)]

# Create a thread for each user registration
threads = []
for username in usernames:
    thread = threading.Thread(target=register_user, args=(username,))
    threads.append(thread)
    thread.start()

start = time.time()
# Wait for all threads to complete
for thread in threads:
    thread.join()
end = time.time()

print(f"Registered {count} out of {test_size} ({count / test_size * 100}%) {(end - start).__round__(4)}sec passed")

count = 0
# Select a user to perform login and subsequent actions
login_user = random.choice(usernames)

# Perform login
login(login_user)

# Check if login was successful and get the token
if tokens:
    token = tokens[login_user]

    # Perform an action that requires authentication, like creating a store
    response = create_store(token)
    global store_id
    # Get the store ID from the response or use a predefined value
    store_id = response.json()["id"]  # Update with the correct key if necessary

    # Add 100 products to the store

    threads = []
    for _ in range(test_size):
        thread = threading.Thread(target=add_product, args=(token, store_id,))
        threads.append(thread)
        thread.start()

    start = time.time()
    # Wait for all threads to complete
    for thread in threads:
        thread.join()
    end = time.time()


    print(f"added {count} products out of {test_size} ({count / test_size * 100}%) {(end - start).__round__(4)}sec passed")

    response2 = add_product(token, store_id)

    global product_id
    product_id = response.json()["id"]
else:
    print("Login failed.")
    exit()

def items_to_cart(username):
    global token
    global user_id
    url = f"{server_url}/api/shoppingcarts/member/{user_id}"
    headers = {
        "Authorization": f"Bearer {token}"
    }
    data = {
    }
    r2 = requests.get(url, json=data, headers=headers, verify=False)
    if r2.status_code > 300:
        print(f"Failed to get shopping cart :{user_id}")
        return

    cartId = r2.json()["id"]
    global product_id
    global store_id
    url = f"{server_url}/api/shoppingcarts/{cartId}/items"
    headers = {
        "Authorization": f"Bearer {token}"
    }
    data = {
        "productId": product_id,
        "storeId": store_id,
        "quantity": 10000
    }
    r3 = requests.post(url, json=data, headers=headers, verify=False)
    if r3.status_code > 300 and r3.text != "Item Is already in basket.":
        print(f"Failed to get shopping cart: {r3.text}")
        return
    global count
    count += 1


threads = []
count = 0
for i in range(test_size): #slow test
    thread = threading.Thread(target=items_to_cart, args=(([usernames[0]])))
    threads.append(thread)
    thread.start()

start = time.time()
# Wait for all threads to complete
for thread in threads:
    thread.join()
end = time.time()

print(f"added {count} items to cart out of {test_size} ({count / test_size * 100}%) {(end - start).__round__(4)}sec passed")

def purchase_test():
    global user_id
    global product_id
    global store_id
    url = f"{server_url}/api/transactions"
    headers = {
        "Authorization": f"Bearer {token}"
    }
    data = {
        "isMember": True,
        "memberId": user_id,
        "billingDetails": {
            "holderFirstName": "string",
            "holderLastName": "string",
            "holderID": "123456789",
            "cardNumber": "1234123412341234",
            "expirationDate": "2025-06-20",
            "cvc": random.randint(100, 999).__str__()
        },
        "deliveryDetails": {
            "receiverFirstName": "string",
            "receiverLastName": "string",
            "receiverAddress": "string",
            "receiverCity": "string",
            "receiverCountry": "string",
            "receiverPostalCode": "strings"
        },
        "transactionItems": [
            {
                "productId": product_id,
                "storeId": store_id,
                "productName": "string",
                "productDescription": "string",
                "quantity": 1,
                "fullPrice": 10
            }
        ],
        "totalPrice": 0
    }
    r = requests.post(url, json=data, headers=headers, verify=False)
    global count
    count += 1
global printed
printed = False
def wait_for_timeout():
    time.sleep(30)
    global printed
    if not printed:
        printed = True
        print(f"timeout, {count} purchases finished {test_size // 10} ({count / test_size * 1000}%) {(end - start).__round__(4)}sec passed")

threads = []
count = 0
for i in range(test_size // 10): #slow test
    thread = threading.Thread(target=purchase_test)
    threads.append(thread)
    thread.start()

threading.Thread(target=wait_for_timeout).start()

start = time.time()
# Wait for all threads to complete
for thread in threads:
    thread.join()
end = time.time()

printed = True

print(f"{count} purchases finished out of {test_size // 10} ({count / test_size * 1000}%) {(end - start).__round__(4)}sec passed")

time.sleep(999999)