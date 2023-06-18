import { CartItem } from "../../../types/appTypes";

export interface CheckoutFormData {
  billingDetails_holderFirstName: string;
  billingDetails_holderLastName: string;
  billingDetails_holderID: string; // length: 9
  billingDetails_cardNumber: string; // length: 16
  billingDetails_expirationDate: string; // format: "2023-06-17"
  billingDetails_cvc: string; // length: 3

  deliveryDetails_receiverFirstName: string;
  deliveryDetails_receiverLastName: string;
  deliveryDetails_receiverAddress: string;
  deliveryDetails_receiverCity: string;
  deliveryDetails_receiverCountry: string;
  deliveryDetails_receiverPostalCode: string; // length: 7
}
