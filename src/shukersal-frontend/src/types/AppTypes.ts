export interface Category {
  id: number;
  name: string;
}

export type MemberRole = "Member" | "Administrator";

export interface Member {
  id: number;
  username: string;
  role: MemberRole;
}

export enum PermissionType {
  IsOwner = 0, // SPECIAL - for delete store (check if parent is null)
  ManageProducts = 1, // for products table
  ManageDiscounts = 3, // for discouts button
  AppointOwner = 4, //
  RemoveOwner = 5, // IGNORE
  AppointManager = 6, // add manager
  EditManagerPermissions = 7, //
  RemoveManager = 8, //
  GetManagerInfo = 11, // for
  ReplyPermission = 12, // IGNORE
  GetHistoryPermission = 13, // transaction history /api/transactions/store/{storeId}
}

export interface Category {
  id: number;
  name: string;
}

export interface Product {
  id: number;
  name: string;
  description: string;
  price: number;
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
  quantity: number;
}

export type CartItem = Omit<ShoppingItem, "id" | "shoppingBasketId">; // for add/edit

export interface ShoppingBasket {
  id: number;
  shoppingCartId: number;
  storeId: number;
  shoppingItems: ShoppingItem[];
}

export interface ShoppingCart {
  id: number;
  memberId: number;
  shoppingBaskets: ShoppingBasket[];
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
  parentManagerId: number | null;
  storePermissions: StorePermission[];
  childManagers: StoreManager[];
  username: string;
}
