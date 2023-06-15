import { useCallback, useEffect, useState } from 'react';
import { demoStores } from './DEMO_DATA_useStores';
import { demoProducts } from './DEMO_DATA_useProducts';
import { useAppSelector } from '../useAppSelector';
import { useAppDispatch } from '../useAppDispatch';
import { ProductPostFormFields, StorePatchFormFields } from '../../types/formTypes';
import { deleteStore, getStore, resetCurrentStore, updateStore, createProduct } from '../../redux/storeSlice';
import { storeProductsApi } from '../../api/storesApi';
import { Product } from '../../types/appTypes';
import { ApiError, ApiListData } from '../../types/apiTypes';

export const useSellerStore = (storeId: number) => {
    const dispatch = useAppDispatch();

    const isLoading = useAppSelector((state) => state.store.isLoading);
    const currentStore = useAppSelector((state) => state.store.currentStore);
    const [products, setProducts] = useState<Product[]>([]);
    const error = useAppSelector((state) => state.store.error);

    const addProductCallback = useCallback(async (formData: ProductPostFormFields) => {
        const response = await dispatch(createProduct({ storeId, product: formData }));
        if (response.meta.requestStatus === 'fulfilled') {
            return true;
        }
        return false;
    }, [dispatch, storeId]);

    const updateStoreCallback = useCallback(async (formData: StorePatchFormFields) => {
        dispatch(resetCurrentStore());
        const response = await dispatch(updateStore({ storeId, newData: formData }));
        if (response.meta.requestStatus === 'fulfilled') {
            return true;
        }
        return false;

    }, [dispatch, storeId]);

    const deleteStoreCallback = useCallback(async () => {
        const response = await dispatch(deleteStore(storeId));
        if (response.meta.requestStatus === 'fulfilled') {
            return true;
        }
        return false;
    }, [dispatch, storeId]);

    const getStoreProducts = useCallback(async (_storeId: number) => {
        storeProductsApi.getAll(_storeId)
        .then((res) => {
            setProducts(res as ApiListData<Product>);
        })
        .catch((res) => {
            setProducts([]);
            console.error('Error while fetching products: ', res as ApiError);
        })
    }, []);

    useEffect(() => {
        dispatch(getStore(storeId));
        getStoreProducts(storeId);
    }, [dispatch, storeId]);

    return {
        store: currentStore,
        isLoading: isLoading,
        error: error,
        products: products,
        managers: demoStores, // TODO: Implement
        
        getStoreProducts: () => getStoreProducts(storeId),
        addProduct: addProductCallback,
        updateStore: updateStoreCallback,
        deleteStore: deleteStoreCallback
    };
}