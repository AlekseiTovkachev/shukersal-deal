export interface Category {
    id: number;
    name: string;
}

export type MemberRole = 'Member' | 'Administrator';

export interface Member {
    id: number;
    username: string;
    role: MemberRole;
}

export enum PermissionType {
    ManagerPermission = 0;
    ManageProductsPermission = 1;
    ManageDiscountsPermission = 3;
    AppointOwnerPermission = 4;
    RemoveOwnerermission = 5;
    AppointManagerPermission = 6;
    EditManagerPermissionsPermission = 7;
    RemoveManagerPermission = 8;
    GetManagerInfoPermission = 11;
    ReplyPermission = 12;
    GetHistoryPermission = 13
}

export interface Category {
    id: number;
    number: string;
}

export interface Product {
    id: number;
    name: string;
    description: string;
    price: string;
    imageUrl: string;
    isListed: boolean;
    unitsInStock: number;
    category: Category;
    storeId: number;
}

export interface PurchaseItem {
    id: number;
    purchaseId: number;
    productId: number;
    storeId: number;
    productName: string;
    productDescription: string;
    quantity: number;
    fullPrice: number;
    finalPrice: number;
}

export interface Purchase {
    id: number;
    memberId: number;
    purchaseDate: string;
    totalPrice: number;
    purchaseItems: PurchaseItem;
}

export interface ShoppingItem {
    id: number;
    shoppingBasketId: number;
    productId: number;
    quantity:number
}

export interface ShoppingBasket {

    id: number;
    shoppingCartId: number;
    storeId: number;
    shoppingItems: ShoppingItem[]
}

export interface ShoppingCart {
    id: number;
    memberId: number;
    shoppingBaskets: ShoppingBasket[]
}

export interface DiscountRule {
    id: number;
    // TODO Implement
}

export interface Store {
    id: number;
    name: string;
    description: string;
    rootManagerId: number;
    //products: Product[]; TODO: Check if removed in BE
    //discountRules: DiscountRule[]; TODO: Check if removed in BE
}

export interface StorePermission {
    id: number;
    permissionType: PermissionType;
    storeManagerId: number;
}

export interface StoreManager {
    id: number;
    storeId: number;
    memberId: number;
    parentManagerId: number;
    storePermissions: StorePermission[];
}