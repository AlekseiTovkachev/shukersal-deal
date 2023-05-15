import { Product } from '../../types/appTypes';

// TODO: Remove this once redux is implemented
export const demoProducts: Product[] = [
    {
        id: 1,
        name: "Wireless Headphones",
        description: "Experience the freedom of wireless music with these high-quality headphones.",
        price: 80,
        imageUrl: "https://picsum.photos/id/1/1000/1000",
        isListed: true,
        unitsInStock: 50,
        category: {
            id: 1,
            name: "Electronics"
        },
        storeId: 1
    },
    {
        id: 2,
        name: "Smartwatch",
        description: "Stay connected and track your fitness with this sleek smartwatch.",
        price: 150,
        imageUrl: "https://picsum.photos/id/2/1000/1000",
        isListed: true,
        unitsInStock: 25,
        category: {
            id: 1,
            name: "Electronics"
        },
        storeId: 1
    },
    {
        id: 3,
        name: "Organic Coffee",
        description: "Start your day off right with this delicious, sustainably-sourced coffee.",
        price: 13,
        imageUrl: "https://picsum.photos/id/3/1000/1000",
        isListed: true,
        unitsInStock: 100,
        category: {
            id: 3,
            name: "Groceries"
        },
        storeId: 1
    },
    {
        id: 4,
        name: "Bamboo Cutting Board",
        description: "Upgrade your kitchen with this eco-friendly bamboo cutting board.",
        price: 25,
        imageUrl: "https://picsum.photos/id/4/1000/1000",
        isListed: true,
        unitsInStock: 20,
        category: {
            id: 4,
            name: "Home"
        },
        storeId: 1
    },
    {
        id: 5,
        name: "Yoga Mat",
        description: "Take your yoga practice to the next level with this high-quality yoga mat.",
        price: 50,
        imageUrl: "https://picsum.photos/id/5/1000/1000",
        isListed: true,
        unitsInStock: 30,
        category: {
            id: 5,
            name: "Sports"
        },
        storeId: 1
    }
];
