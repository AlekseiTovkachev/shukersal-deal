import { SELLER_ID_1 } from './DEMO_DATA_useSeller';
import { useCallback, useEffect, useState } from 'react';
import { demoStores } from './DEMO_DATA_useStores';
import { useAppDispatch } from '../useAppDispatch';
import { useAppSelector } from '../useAppSelector';
import { StorePostFormFields } from '../../types/formTypes';
import { createStore, getAllStore } from '../../redux/storeSlice';

export const useSeller = () => {
    // TODO: Implement

    const dispatch = useAppDispatch();

    const isLoadingStoreService = useAppSelector((state) => state.store.isLoading);
    const storeServiceError = useAppSelector((state) => state.store.error);

    const isLoading = isLoadingStoreService // || isLoadingManagerService

    const getStoresCallback = useCallback(async () => {
        dispatch(getAllStore());
    }, [dispatch]);

    const createStoreCallback = useCallback(async (formData: StorePostFormFields) => {
        const response = await dispatch(createStore(formData));
        if (response.meta.requestStatus === 'fulfilled') {
            return true;
        }
        return false;
    }, [dispatch]);
    
    return {
        sellerIds: [SELLER_ID_1], // TODO: Implement
        isLoading: isLoading,
        error: storeServiceError, // || managerServiceError
        stores: demoStores, // TODO: Implement

        refreshStores: getStoresCallback,
        createStore: createStoreCallback,
    };
}