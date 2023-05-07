export const API_URL = 'https://localhost:7258/api/';
export const API_DATE_TIME_FORMAT = 'YYYY-MM-DDTHH:mm:ssZ';
export const APP_CURRENCY_SIGN = "$";

export interface LocalStorageEntry<T> {
    name: string, // The local storage name
    defaultValue: T
};
type Entry<T> = LocalStorageEntry<T> // Syntactic Sugar for internal use

export interface LocalStorage {
    auth: {
        currentMemberData: Entry<string | null>
    },
    settings: {
        loginPage: {
            rememberMe: Entry<boolean>
        }
    }
};

export const localStorageValues: LocalStorage = {
    auth: {
        currentMemberData: {
            name: 'auth.currentMemberData',
            defaultValue: null
        },
    },
    settings: {
        loginPage: {
            rememberMe: {
                name: 'settings.loginPage.rememberMe',
                defaultValue: false
            }
        }
    }
};