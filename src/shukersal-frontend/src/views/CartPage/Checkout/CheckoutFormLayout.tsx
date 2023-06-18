import { useMemo } from "react";

import { Controller, useFormContext } from "react-hook-form";

import {
  Typography,
  Divider,
  TextField,
  Grid,
  Box,
  useTheme,
  useMediaQuery,
  FormControlLabel,
  Checkbox,
} from "@mui/material";
import { StorePostFormFields } from "../../../types/formTypes";
import { CheckoutFormData } from "./types";

export const CheckoutFormLayout = () => {
  const theme = useTheme();

  const form = useFormContext<CheckoutFormData>();
  const formValues = form.getValues();

  return (
    <>
      <Grid item xs={12}>
        <Typography variant="h5">Billing Details:</Typography>
      </Grid>
      {/* ---------------------- billingDetails_holderFirstName ------------------------- */}
      <Grid item xs={6}>
        <Controller
          control={form.control}
          name={"billingDetails_holderFirstName"}
          rules={{
            required: {
              value: true,
              message: "First Name is required",
            },
          }}
          render={({ field, fieldState, formState }) => {
            return (
              <TextField
                type="text"
                name="name"
                label="First Name"
                value={field.value}
                fullWidth
                size="small"
                onChange={(e) => {
                  field.onChange(e.target.value);
                }}
                onBlur={field.onBlur}
                ref={field.ref}
                error={!!fieldState.error}
                helperText={fieldState.error?.message}
              />
            );
          }}
        />
      </Grid>
      {/* ---------------------- billingDetails_holderLastName ------------------------- */}
      <Grid item xs={6}>
        <Controller
          control={form.control}
          name={"billingDetails_holderLastName"}
          rules={{
            required: {
              value: true,
              message: "Last Name is required",
            },
          }}
          render={({ field, fieldState, formState }) => {
            return (
              <TextField
                type="text"
                name="name"
                label="Last Name"
                value={field.value}
                fullWidth
                size="small"
                onChange={(e) => {
                  field.onChange(e.target.value);
                }}
                onBlur={field.onBlur}
                ref={field.ref}
                error={!!fieldState.error}
                helperText={fieldState.error?.message}
              />
            );
          }}
        />
      </Grid>
      {/* ---------------------- billingDetails_holderID ------------------------- */}
      <Grid item xs={12}>
        <Controller
          control={form.control}
          name={"billingDetails_holderID"}
          rules={{
            required: {
              value: true,
              message: "ID is required",
            },
            pattern: {
              value: /^\w{9}$/,
              message: "ID must be exactly 9 digits",
            },
          }}
          render={({ field, fieldState, formState }) => {
            return (
              <TextField
                type="text"
                name="name"
                label="ID"
                value={field.value}
                fullWidth
                size="small"
                onChange={(e) => {
                  field.onChange(e.target.value);
                }}
                onBlur={field.onBlur}
                ref={field.ref}
                error={!!fieldState.error}
                helperText={fieldState.error?.message}
              />
            );
          }}
        />
      </Grid>
      {/* ---------------------- billingDetails_cardNumber ------------------------- */}
      <Grid item xs={12}>
        <Controller
          control={form.control}
          name={"billingDetails_cardNumber"}
          rules={{
            required: {
              value: true,
              message: "Card Number is required",
            },

            pattern: {
              value: /^\w{16,19}$/,
              message: "Card Number must have 16 to 19 digits",
            },
          }}
          render={({ field, fieldState, formState }) => {
            return (
              <TextField
                type="text"
                name="name"
                label="Card Number"
                value={field.value}
                fullWidth
                size="small"
                onChange={(e) => {
                  field.onChange(e.target.value);
                }}
                onBlur={field.onBlur}
                ref={field.ref}
                error={!!fieldState.error}
                helperText={fieldState.error?.message}
              />
            );
          }}
        />
      </Grid>
      {/* ---------------------- billingDetails_expirationDate ------------------------- */}
      <Grid item xs={6}>
        <Controller
          control={form.control}
          name={"billingDetails_expirationDate"}
          rules={{
            required: {
              value: true,
              message: "Expiration Date is required",
            },
            pattern: {
              value: /^\d{4}-\d{2}-\d{2}$/,
              message: "Invalid date format. Please use YYYY-MM-DD",
            },
          }}
          render={({ field, fieldState, formState }) => {
            return (
              <TextField
                type="text"
                name="name"
                label="Expiration Date"
                value={field.value}
                fullWidth
                size="small"
                onChange={(e) => {
                  field.onChange(e.target.value);
                }}
                onBlur={field.onBlur}
                ref={field.ref}
                error={!!fieldState.error}
                helperText={fieldState.error?.message}
              />
            );
          }}
        />
      </Grid>
      {/* ---------------------- billingDetails_cvc ------------------------- */}
      <Grid item xs={6}>
        <Controller
          control={form.control}
          name={"billingDetails_cvc"}
          rules={{
            required: {
              value: true,
              message: "CVC is required",
            },
            pattern: { 
              value: /^\w{3}$/,
              message: "ID must be exactly 3 digits",
            },
          }}
          render={({ field, fieldState, formState }) => {
            return (
              <TextField
                type="text"
                name="name"
                label="CVC"
                value={field.value}
                fullWidth
                size="small"
                onChange={(e) => {
                  field.onChange(e.target.value);
                }}
                onBlur={field.onBlur}
                ref={field.ref}
                error={!!fieldState.error}
                helperText={fieldState.error?.message}
              />
            );
          }}
        />
      </Grid>
      <Grid item xs={12}>
        <Divider orientation="horizontal" />
      </Grid>
      <Grid item xs={12}>
        <Typography variant="h5">Delivery Details:</Typography>
      </Grid>
      {/* ---------------------- deliveryDetails_receiverFirstName ------------------------- */}
      <Grid item xs={6}>
        <Controller
          control={form.control}
          name={"deliveryDetails_receiverFirstName"}
          rules={{
            required: {
              value: true,
              message: "First Name is required",
            },
          }}
          render={({ field, fieldState, formState }) => {
            return (
              <TextField
                type="text"
                name="name"
                label="First Name"
                value={field.value}
                fullWidth
                size="small"
                onChange={(e) => {
                  field.onChange(e.target.value);
                }}
                onBlur={field.onBlur}
                ref={field.ref}
                error={!!fieldState.error}
                helperText={fieldState.error?.message}
              />
            );
          }}
        />
      </Grid>
      {/* ---------------------- deliveryDetails_receiverLastName ------------------------- */}
      <Grid item xs={6}>
        <Controller
          control={form.control}
          name={"deliveryDetails_receiverLastName"}
          rules={{
            required: {
              value: true,
              message: "Last Name is required",
            },
          }}
          render={({ field, fieldState, formState }) => {
            return (
              <TextField
                type="text"
                name="name"
                label="Last Name"
                value={field.value}
                fullWidth
                size="small"
                onChange={(e) => {
                  field.onChange(e.target.value);
                }}
                onBlur={field.onBlur}
                ref={field.ref}
                error={!!fieldState.error}
                helperText={fieldState.error?.message}
              />
            );
          }}
        />
      </Grid>
      {/* ---------------------- deliveryDetails_receiverAddress ------------------------- */}
      <Grid item xs={12}>
        <Controller
          control={form.control}
          name={"deliveryDetails_receiverAddress"}
          rules={{
            required: {
              value: true,
              message: "Address is required",
            },
          }}
          render={({ field, fieldState, formState }) => {
            return (
              <TextField
                type="text"
                name="name"
                label="Address"
                value={field.value}
                fullWidth
                size="small"
                onChange={(e) => {
                  field.onChange(e.target.value);
                }}
                onBlur={field.onBlur}
                ref={field.ref}
                error={!!fieldState.error}
                helperText={fieldState.error?.message}
              />
            );
          }}
        />
      </Grid>
      {/* ---------------------- deliveryDetails_receiverCity ------------------------- */}
      <Grid item xs={4}>
        <Controller
          control={form.control}
          name={"deliveryDetails_receiverCity"}
          rules={{
            required: {
              value: true,
              message: "City is required",
            },
          }}
          render={({ field, fieldState, formState }) => {
            return (
              <TextField
                type="text"
                name="name"
                label="City"
                value={field.value}
                fullWidth
                size="small"
                onChange={(e) => {
                  field.onChange(e.target.value);
                }}
                onBlur={field.onBlur}
                ref={field.ref}
                error={!!fieldState.error}
                helperText={fieldState.error?.message}
              />
            );
          }}
        />
      </Grid>
      {/* ---------------------- deliveryDetails_receiverCountry ------------------------- */}
      <Grid item xs={4}>
        <Controller
          control={form.control}
          name={"deliveryDetails_receiverCountry"}
          rules={{
            required: {
              value: true,
              message: "Country is required",
            },
          }}
          render={({ field, fieldState, formState }) => {
            return (
              <TextField
                type="text"
                name="name"
                label="Country"
                value={field.value}
                fullWidth
                size="small"
                onChange={(e) => {
                  field.onChange(e.target.value);
                }}
                onBlur={field.onBlur}
                ref={field.ref}
                error={!!fieldState.error}
                helperText={fieldState.error?.message}
              />
            );
          }}
        />
      </Grid>
      {/* ---------------------- deliveryDetails_receiverPostalCode ------------------------- */}
      <Grid item xs={4}>
        <Controller
          control={form.control}
          name={"deliveryDetails_receiverPostalCode"}
          rules={{
            required: {
              value: true,
              message: "Postal Code is required",
            },
            pattern: {
              value: /^\w{7}$/,
              message: "Postal Code must be exactly 7 digits",
            },
          }}
          render={({ field, fieldState, formState }) => {
            return (
              <TextField
                type="text"
                name="name"
                label="Postal Code"
                value={field.value}
                fullWidth
                size="small"
                onChange={(e) => {
                  field.onChange(e.target.value);
                }}
                onBlur={field.onBlur}
                ref={field.ref}
                error={!!fieldState.error}
                helperText={fieldState.error?.message}
              />
            );
          }}
        />
      </Grid>
    </>
  );
};
