import { SELLER_ID_1 } from './DEMO_DATA_useSeller';
import { useEffect, useState } from 'react';
import { demoStores } from './DEMO_DATA_useStores';
import { Store } from '../../types/appTypes';
import { demoProducts } from './DEMO_DATA_useProducts';

export const useSellerStore = (storeId: number) => {
    // TODO: Implement
    const [isLoading, setIsLoading] = useState<boolean>(true);
    useEffect(() => {
        setIsLoading(true);
        setTimeout(() => {
            setIsLoading(false);
        }, 1000);
    }, []);
    return {
        store: demoStores[0],
        isLoading: isLoading,
        products: demoProducts,
        managers: demoStores
    };
}