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
import { AddProductFormLayout } from "./AddProductFormLayout";
import { errorToString } from "../../../util";
import { useSellerStore } from "../../../hooks/data/useSellerStore";

interface AddProductDialogProps {
  storeId: number;
}

export const AddProductDialog = NiceModal.create(
  ({ storeId }: AddProductDialogProps) => {
    const modal = useModal();
    const sellerStoreData = useSellerStore(storeId);

    const form = useForm<ProductPostFormFields>({
      defaultValues: {
        name: "",
        description: "",
        price: 0,
        unitsInStock: 0,
        imageUrl: undefined,
        isListed: true,
        categoryId: 1,
      },
    });
    const handleClose = useCallback(() => {
      modal.hide();
    }, [modal]);

    const handleSubmit = useCallback(async () => {
      const isSuccess = await sellerStoreData.addProduct(form.getValues());
      // sellerData.refreshMyStores();
      if (isSuccess) {
        modal.resolve();
        handleClose();
      }
    }, [sellerStoreData, form, handleClose]);

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
              <DialogTitle>Add Product</DialogTitle>
              <DialogContent>
                <Grid container rowSpacing={2} width="100%">
                  <AddProductFormLayout />
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
                    Add Product
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
