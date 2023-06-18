const BASE_URL = "https://localhost:7258/";
// const BASE_URL = "http://localhost:5130/";
export const CHAT_URL = `${BASE_URL}chatHub`;
export const API_URL = `${BASE_URL}api/`;
export const API_DATE_TIME_FORMAT = "YYYY-MM-DDTHH:mm:ss.SSSZ";
export const DISPLAY_DATE_TIME_FORMAT = "DD/MM/YYYY - HH:mm:ss";
export const APP_CURRENCY_SIGN = "$";

export interface LocalStorageEntry<T> {
  name: string; // The local storage name
  defaultValue: T;
}
type Entry<T> = LocalStorageEntry<T>; // Syntactic Sugar for internal use

export interface LocalStorage {
  auth: {
    currentMemberData: Entry<string | null>;
  };
  cartItems: Entry<string>;
  // settings: {
  //     loginPage: {
  //         rememberMe: Entry<boolean>
  //     }
  // }
}

export const localStorageValues: LocalStorage = {
  auth: {
    currentMemberData: {
      name: "auth.currentMemberData",
      defaultValue: null,
    },
  },
  cartItems: {
    name: "cartItems",
    defaultValue: "[]",
  },
  // settings: {
  //     loginPage: {
  //         rememberMe: {
  //             name: 'settings.loginPage.rememberMe',
  //             defaultValue: false
  //         }
  //     }
  // }
};
