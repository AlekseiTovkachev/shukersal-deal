import _ from "lodash";
import { Product } from "../../types/appTypes"

export const filterProduct = (p: Product, searchText: string): boolean => {
    const lowerSearchText = searchText.toLowerCase();
    
    // Check if any property of the product contains the search text as substring.
    return _.some(p, (value, key) => {
        if (typeof value === 'string') {
            return value.toLowerCase().includes(lowerSearchText);
        } else if (key === 'category' && value && typeof value === 'object') {
            return value.name.toLowerCase().includes(lowerSearchText);
        }
        return false;
    });
};