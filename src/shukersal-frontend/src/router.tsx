import { createBrowserRouter } from 'react-router-dom';

import { MainPage } from './views/MainPage';
import { BuyerPage } from './views/BuyerPage/BuyerPage';
import { SellerPage } from './views/SellerPage/SellerPage';
import { AdminPage } from './views/AdminPage/AdminPage';

import { LoginPage } from './views/LoginPage/LoginPage';
import { ForgotPasswordPanel } from './views/LoginPage/ForgotPasswordPanel/ForgotPasswordPanel';
import { RegisterPanel } from './views/LoginPage/RegisterPanel/RegisterPanel';
import { LoginPanel } from './views/LoginPage/LoginPanel/LoginPanel';
import { CartPage } from './views/CartPage/CartPage';
import { StoresPage } from './views/StoresPage/StoresPage';
import { StorePage } from './views/StorePage/StorePage';
import { ProductsPage } from './views/ProductsPage/ProductsPage';
import { ProductPage } from './views/ProductPage/ProductPage';
import { SellerStorePage } from './views/SellerPage/SellerStorePage';
import { NotFoundPage } from './views/NotFoundPage/NotFoundPage';
import { SellerStoreDiscountsPage } from './views/SellerPage/SellerStoreDiscountsPage/SellerStoreDiscountsPage';
import { SellerStoreManagersPage } from './views/SellerPage/SellerStoreManagersPage/SellerStoreManagersPage';

export const router = createBrowserRouter([
    {
        path: '/',
        element: <MainPage />,
        children: [
            {
                path: '',
                element: <BuyerPage />,
            },
            {
                path: 'products',
                children: [
                    {
                        path: '',
                        element: <ProductsPage />
                    },
                    {
                        path: ':productId',
                        element: <ProductPage />
                    }
                ]
            },
            {
                path: 'stores',
                children: [
                    {
                        path: '',
                        element: <StoresPage />
                    },
                    {
                        path: ':storeId',
                        element: <StorePage />
                    }
                ]
            },
            {
                path: 'cart',
                element: <CartPage />

            },
            {
                path: 'admin',
                element: <AdminPage />
            },
            {
                path: 'seller',
                children: [
                    {
                        path: '',
                        element: <SellerPage />
                    },
                    {
                        path: 'stores/:storeId',
                        children: [
                            {
                                path: '',
                                element: <SellerStorePage />
                            },
                            {
                                path: 'managers',
                                element: <SellerStoreManagersPage />
                            },
                            {
                                path: 'discounts',
                                element: <SellerStoreDiscountsPage />
                            }
                        ]
                    }
                ]
            },
            {
                path: 'login',
                element: <LoginPage />,
                children: [
                    {
                        path: '',
                        element: <LoginPanel />
                    },
                    {
                        path: 'register',
                        element: <RegisterPanel />
                    },
                    {
                        path: 'forgotpassword',
                        element: <ForgotPasswordPanel />
                    }
                ]
            },
            {
                path: '*',
                element: <NotFoundPage />
            }
        ],
    },
]);