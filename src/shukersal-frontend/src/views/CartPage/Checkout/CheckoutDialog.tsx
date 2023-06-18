import { useCallback } from "react";

import NiceModal, {
  muiDialog,
  muiDialogV5,
  useModal,
} from "@ebay/nice-modal-react";

import {
  Alert,
  Button,
  Dialog,
  DialogActions,
  DialogContent,
  DialogContentText,
  DialogTitle,
  FormControl,
  Grid,
  TextField,
  Typography,
} from "@mui/material";
import { useSeller } from "../../../hooks/data/useSeller";
import { useNavigate } from "react-router-dom";
import { StorePostFormFields } from "../../../types/formTypes";
import { FormProvider, useForm } from "react-hook-form";
import { FlexSpacer } from "../../../components/FlexSpacer";
import { LoadingButton } from "@mui/lab";
import { CheckoutFormLayout } from "./CheckoutFormLayout";
import { errorToString } from "../../../util";
import { CheckoutFormData } from "./types";
import { useCheckout } from "./useCheckout";
import { TransactionPostData } from "../../../api/cartApi";
import { useAuth } from "../../../hooks/useAuth";
import { CartItem, Member } from "../../../types/appTypes";
import dayjs from "dayjs";
import { API_DATE_TIME_FORMAT } from "../../../_configuration";
import { SuccessDialog } from "../../../components/SuccessDialog";
import { clearCart } from "../../../redux/cartSlice";
import { useAppDispatch } from "../../../hooks/useAppDispatch";

const makeTransaction = (
  cartItems: CartItem[],
  totalPrice: number,
  formData: CheckoutFormData,
  currentMember?: Member
): TransactionPostData => {
  return {
    isMember: !!currentMember,
    memberId: currentMember?.id ?? undefined,
    transactionDate: dayjs().format(API_DATE_TIME_FORMAT),
    billingDetails: {
      holderFirstName: formData.billingDetails_holderFirstName,
      holderLastName: formData.billingDetails_holderLastName,
      holderID: formData.billingDetails_holderID,
      cardNumber: formData.billingDetails_cardNumber,
      expirationDate: formData.billingDetails_expirationDate,
      cvc: formData.billingDetails_cvc,
    },
    deliveryDetails: {
      receiverFirstName: formData.deliveryDetails_receiverFirstName,
      receiverLastName: formData.deliveryDetails_receiverLastName,
      receiverAddress: formData.deliveryDetails_receiverAddress,
      receiverCity: formData.deliveryDetails_receiverCity,
      receiverCountry: formData.deliveryDetails_receiverCountry,
      receiverPostalCode: formData.deliveryDetails_receiverPostalCode,
    },
    transactionItems: cartItems,
    totalPrice: totalPrice,
  };
};

export const CheckoutDialog = NiceModal.create(
  ({
    cartItems,
    totalPrice,
  }: {
    cartItems: CartItem[];
    totalPrice: number;
  }) => {
    const dispatch = useAppDispatch();
    const modal = useModal();

    const checkoutData = useCheckout();

    const authData = useAuth();
    const currentMember = authData.currentMemberData?.currentMember;

    const form = useForm<CheckoutFormData>({
      defaultValues: {
        billingDetails_holderFirstName: "",
        billingDetails_holderLastName: "",
        billingDetails_holderID: "", // length: 9
        billingDetails_cardNumber: "", // length: 16
        billingDetails_expirationDate: "", // format: "2023-06-17"
        billingDetails_cvc: "", // length: 3

        deliveryDetails_receiverFirstName: "",
        deliveryDetails_receiverLastName: "",
        deliveryDetails_receiverAddress: "",
        deliveryDetails_receiverCity: "",
        deliveryDetails_receiverCountry: "",
        deliveryDetails_receiverPostalCode: "", // length: 7
      },
    });

    const handleClose = useCallback(() => {
      modal.hide();
    }, [modal]);

    const handleSubmit = useCallback(async () => {
      const isSuccess = await checkoutData.submitTransaction(
        makeTransaction(cartItems, totalPrice, form.getValues(), currentMember)
      );
      if (isSuccess) {
        NiceModal.show(SuccessDialog, {
          title: "Success",
          body: (
            <Alert severity="success" variant="filled">
              Transaction Complete Successfully!
            </Alert>
          ),
          onClose: async () => {
            dispatch(clearCart());
            window.location.reload();
          },
        });
        handleClose();
      }
    }, [form, handleClose, currentMember, cartItems, totalPrice]);

    return (
      <Dialog {...muiDialogV5(modal)}>
        <FormProvider {...form}>
          <form
            onSubmit={(e) => {
              e.preventDefault();
              form.handleSubmit(handleSubmit)();
            }}
          >
            <FormControl
              sx={{
                display: "flex",
                flexDirection: "column",
                width: "100%",
                height: "100%",
                boxSizing: "border-box",
                padding: 2,
              }}
            >
              <DialogTitle variant="h3">Checkout</DialogTitle>
              <DialogContent>
                <Grid container spacing={2} width="100%">
                  <CheckoutFormLayout />
                </Grid>

                {checkoutData.error && (
                  <Alert variant="outlined" severity="error" sx={{ my: 2 }}>
                    Error: {checkoutData.error}
                  </Alert>
                )}
                {/* <FlexSpacer minHeight={25} /> */}
                <DialogActions>
                  <Button onClick={handleClose}>Cancel</Button>
                  <LoadingButton
                    variant="contained"
                    color="secondary"
                    loading={checkoutData.isLoading}
                    type="submit"
                    // onClick={form.handleSubmit(handleLogin)}
                    //disabled={(() => {console.log('formState: ', form.formState); return !form.formState.isValid})()}
                  >
                    Checkout
                  </LoadingButton>
                </DialogActions>
              </DialogContent>
            </FormControl>
          </form>
        </FormProvider>
      </Dialog>
    );
  }
);
