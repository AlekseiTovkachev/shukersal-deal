export interface LoginFormFields {
    username: string,
    password: string,
    rememberMe: boolean
}

export interface RegisterFormFields {
    username: string,
    password: string,
    confirmPassword: string
}

export interface StorePostFormFields {
    name: string,
    description: string
}

export interface StorePatchFormFields {
    name?: string,
    description?: string
}

export interface ProductPostFormFields {
    name: string;
    description: string;
    price: number;
    unitsInStock: number;
    imageUrl?: string | null;
    isListed: boolean;
    categoryId: number;
  }