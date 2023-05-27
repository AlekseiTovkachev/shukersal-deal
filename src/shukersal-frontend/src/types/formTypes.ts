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