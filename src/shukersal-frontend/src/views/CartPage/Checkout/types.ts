interface CheckoutData {
  isMember: true;
  memberId: 1;
  transactionDate: string;
  billingDetails: {
    holderFirstName: string;
    holderLastName: string;
    holderID: string; // length: 9
    cardNumber: string; // length: 16
    expirationDate: string; // format: "2023-06-17"
    cvc: string; // length: 3
  };
  deliveryDetails: {
    receiverFirstName: string;
    receiverLastName: string;
    receiverAddress: string;
    receiverCity: string;
    receiverCountry: string;
    receiverPostalCode: string; // length: 7
  };
  transactionItems: {
    productId: number; // THIS
    storeId: number;
    productName: string;
    productDescription: string;
    quantity: number; // THIS
    fullPrice: number;
  }[];
  totalPrice: number;
}
