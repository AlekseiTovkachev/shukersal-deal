import { useMemo } from 'react';

import { Controller, useFormContext } from 'react-hook-form';

import { Typography, Divider, TextField, Grid, Box, useTheme, useMediaQuery, FormControlLabel, Checkbox } from '@mui/material';
import { StorePostFormFields } from '../../../types/formTypes';

export const CheckoutFormLayout = () => {
    const theme = useTheme();
    const isScreenLessThanMedium = useMediaQuery(theme.breakpoints.down("md"));

    const form = useFormContext<StorePostFormFields>();
    const formValues = form.getValues();

    return (<>
        <Grid item xs={12}>
            <Controller
                control={form.control}
                name={'name'}
                rules={{
                    required: {
                        value: true,
                        message: 'Name is required'
                    }
                }}
                render={({ field, fieldState, formState }) => {
                    return <TextField
                        type="text"
                        name="name"
                        placeholder="Name"
                        value={field.value}
                        fullWidth
                        size='small'
                        onChange={(e) => {
                            field.onChange(e.target.value)
                        }}
                        onBlur={field.onBlur}
                        ref={field.ref}
                        error={!!fieldState.error}
                        helperText={fieldState.error?.message}

                        sx={(theme) => ({
                            input: {
                                // color: theme.palette.primary.contrastText
                            }
                        })}
                    />
                }}
            />
        </Grid>
        <Grid item xs={12}>
            <Controller
                control={form.control}
                name={'description'}
                rules={{
                    required: {
                        value: true,
                        message: 'Description is required'
                    },
                }}
                render={({ field, fieldState, formState }) => {
                    return <TextField
                        type=""
                        multiline
                        name="description"
                        placeholder="Description"
                        value={field.value}
                        fullWidth
                        size='small'
                        onChange={(e) => {
                            field.onChange(e.target.value)
                        }}
                        onBlur={field.onBlur}
                        ref={field.ref}
                        error={!!fieldState.error}
                        helperText={fieldState.error?.message}

                        sx={(theme) => ({
                            input: {
                                // color: theme.palette.primary.contrastText
                            }
                        })}
                    />
                }}
            />
        </Grid>
    </>);
};