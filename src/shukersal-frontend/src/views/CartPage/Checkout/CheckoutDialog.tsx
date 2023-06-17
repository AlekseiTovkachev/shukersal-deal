import { useCallback } from 'react';

import NiceModal, {
  muiDialog,
  muiDialogV5,
  useModal,
} from '@ebay/nice-modal-react';

import { Button, Dialog, DialogActions, DialogContent, DialogContentText, DialogTitle, FormControl, Grid, TextField, Typography } from '@mui/material';
import { useSeller } from '../../../hooks/data/useSeller';
import { useNavigate } from 'react-router-dom';
import { StorePostFormFields } from '../../../types/formTypes';
import { FormProvider, useForm } from 'react-hook-form';
import { FlexSpacer } from '../../../components/FlexSpacer';
import { LoadingButton } from '@mui/lab';
import { CheckoutFormLayout } from './CheckoutFormLayout';
import { errorToString } from '../../../util';


export const CheckoutDialog = NiceModal.create(() => {
  const modal = useModal();
  const sellerData = useSeller();

  const form = useForm<StorePostFormFields>({
    defaultValues: {
      name: '',
      description: ''
    }
  });
  const handleClose = useCallback(() => {
    modal.hide();
  }, [modal]);

  const handleSubmit = useCallback(async () => {
    const isSuccess = await sellerData.createStore(form.getValues());
    sellerData.refreshMyStores();
    if (isSuccess) {
      handleClose();
    }
  }, [sellerData, form, handleClose]);

  return (
    <Dialog {...muiDialogV5(modal)}>
      <FormProvider {...form}>
        <form onSubmit={(e) => {
          e.preventDefault();
          form.handleSubmit(handleSubmit)();
        }}>
          <FormControl
            sx={{
              display: 'flex',
              flexDirection: 'column',
              width: '100%',
              height: '100%',
              boxSizing: 'border-box',
              padding: 2
            }}
          >
            <DialogTitle>Open Store</DialogTitle>
            <DialogContent>

              <Grid
                container
                rowSpacing={2}
                width="100%"
              >
                <CheckoutFormLayout />
              </Grid>
              {sellerData.error && <Typography variant="body1" color="error">
                {errorToString(sellerData.error.message)}
              </Typography>}
              <FlexSpacer minHeight={100} />
              
              <DialogActions>
                <Button onClick={handleClose}>Cancel</Button>
                <LoadingButton
                  variant="contained"
                  color="secondary"
                  loading={sellerData.isLoading}
                  type="submit"
                // onClick={form.handleSubmit(handleLogin)}
                //disabled={(() => {console.log('formState: ', form.formState); return !form.formState.isValid})()}
                >Open Store</LoadingButton>
              </DialogActions>
            </DialogContent>
          </FormControl >
        </form>
      </FormProvider >
    </Dialog>
  );
});