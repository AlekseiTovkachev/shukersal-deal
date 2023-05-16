import { useCallback, useEffect, useState } from 'react';
import { demoStores } from './DEMO_DATA_useStores';
import { Store } from '../../types/appTypes';
import { demoProducts } from './DEMO_DATA_useProducts';
import { useAppSelector } from '../useAppSelector';
import { useAppDispatch } from '../useAppDispatch';
import { StorePatchFormFields, StorePostFormFields } from '../../types/formTypes';
import { createStore, deleteStore, getStore, updateStore } from '../../redux/storeSlice';

export const useSellerStore = (storeId: number) => {
    const dispatch = useAppDispatch();

    const isLoading = useAppSelector((state) => state.store.isLoading);
    const currentStore = useAppSelector((state) => state.store.currentStore);
    const error = useAppSelector((state) => state.store.error);


    

    const updateStoreCallback = useCallback(async (formData: StorePatchFormFields) => {
        const response = await dispatch(updateStore({ storeId, newData: formData }));
        if (response.meta.requestStatus === 'fulfilled') {
            return true;
        }
        return false;

    }, [dispatch]);

    const deleteStoreCallback = useCallback(async () => {
        const response = await dispatch(deleteStore(storeId));
        if (response.meta.requestStatus === 'fulfilled') {
            return true;
        }
        return false;
    }, [dispatch]);

    useEffect(() => {
        dispatch(getStore(storeId));
    }, [storeId]);

    return {
        store: currentStore,
        isLoading: isLoading,
        error: error,
        products: demoProducts, // TODO: Implement
        managers: demoStores, // TODO: Implement

        updateStore: updateStoreCallback,
        deleteStore: deleteStoreCallback
    };
}