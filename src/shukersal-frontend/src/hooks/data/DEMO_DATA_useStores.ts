import { Store } from '../../types/appTypes';
import { SELLER_ID_1 } from './DEMO_DATA_useSeller';

// TODO: Remove this once redux is implemented
export const demoStores: Store[] = [
    {
      id: 1,
      name: "ElectroMart",
      description: "Your one-stop shop for all electronic gadgets.",
      rootManagerId: SELLER_ID_1
    },
    {
      id: 2,
      name: "FashionHub",
      description: "The trendiest fashion store in town.",
      rootManagerId: SELLER_ID_1
    },
    {
      id: 3,
      name: "BookWorm",
      description: "A paradise for book lovers.",
      rootManagerId: SELLER_ID_1
    },
    {
      id: 4,
      name: "SportsZone",
      description: "Where champions gear up.",
      rootManagerId: SELLER_ID_1
    },
    {
      id: 5,
      name: "PetParadise",
      description: "Everything your furry friends need.",
      rootManagerId: SELLER_ID_1
    }
  ];