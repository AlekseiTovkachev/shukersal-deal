import { SELLER_ID_1 } from './DEMO_DATA_useSeller';
import { useEffect, useState } from 'react';
import { demoStores } from './DEMO_DATA_useStores';

export const useSeller = () => {
    // TODO: Implement
    const [isLoading, setIsLoading] = useState<boolean>(true);
    useEffect(() => {
        setIsLoading(true);
        setTimeout(() => {
            setIsLoading(false);
        }, 1000);
    }, []);
    return {
        sellerIds: [SELLER_ID_1],
        isLoading: isLoading,
        stores: demoStores,
    };
}