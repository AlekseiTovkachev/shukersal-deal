import { useCallback } from "react";

import NiceModal, {
  muiDialog,
  muiDialogV5,
  useModal,
} from "@ebay/nice-modal-react";

import {
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
import {
  ProductPostFormFields,
  StorePostFormFields,
} from "../../../types/formTypes";
import { FormProvider, useForm } from "react-hook-form";
import { FlexSpacer } from "../../../components/FlexSpacer";
import { LoadingButton } from "@mui/lab";
import { EditProductFormLayout } from "./EditProductFormLayout";
import { errorToString } from "../../../util";
import { useSellerStore } from "../../../hooks/data/useSellerStore";
import { Product } from "../../../types/appTypes";

interface EditProductDialogProps {
  initProduct: Product,
  storeId: number;
  productId: number;
}

export const EditProductDialog = NiceModal.create(
  ({ initProduct, storeId, productId}: EditProductDialogProps) => {
    const modal = useModal();
    const sellerStoreData = useSellerStore(storeId);

    const form = useForm<Partial<Product>>({
      defaultValues: {
        name: initProduct.name,
        description: initProduct.description,
        price: initProduct.price,
        unitsInStock: initProduct.unitsInStock,
        imageUrl: initProduct.imageUrl,
        // isListed: initProduct.isListed,
        // categoryId: 1,
      },
    });
    const handleClose = useCallback(() => {
      modal.hide();
    }, [modal]);

    const handleSubmit = useCallback(async () => {
      const isSuccess = await sellerStoreData.editProduct(productId, form.getValues());
      // sellerData.refreshMyStores();
      if (isSuccess) {
        modal.resolve();
        handleClose();
      }
    }, [productId, sellerStoreData, form, handleClose]);

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
              <DialogTitle>Edit Product</DialogTitle>
              <DialogContent>
                <Grid container rowSpacing={2} width="100%">
                  <EditProductFormLayout />
                </Grid>
                {sellerStoreData.error && (
                  <Typography variant="body1" color="error">
                    {errorToString(sellerStoreData.error.message)}
                  </Typography>
                )}
                <FlexSpacer minHeight={100} />

                <DialogActions>
                  <Button onClick={handleClose}>Cancel</Button>
                  <LoadingButton
                    variant="contained"
                    color="secondary"
                    loading={sellerStoreData.isLoading}
                    type="submit"
                    // onClick={form.handleSubmit(handleLogin)}
                    //disabled={(() => {console.log('formState: ', form.formState); return !form.formState.isValid})()}
                  >
                    Confirm
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
