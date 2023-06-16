import { useMemo } from 'react';

import { Controller, useFormContext } from 'react-hook-form';

import { Typography, Divider, TextField, Grid, Box, useTheme, useMediaQuery, FormControlLabel, Checkbox, FormControl, InputLabel, Select, MenuItem } from '@mui/material';
import { ProductPostFormFields } from '../../../types/formTypes';
import { APP_CATEGORIES } from '../../../constants';
import { Product } from '../../../types/appTypes';

export const EditProductFormLayout = () => {
    const theme = useTheme();
    const isScreenLessThanMedium = useMediaQuery(theme.breakpoints.down("md"));
    
    const form = useFormContext<Partial<Product>>();
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
                    },
                    minLength: {
                        value: 3,
                        message: 'Name length must be at least 3'
                    },
                    maxLength: {
                        value: 100,
                        message: 'Name length must be at most 100'
                    }
                }}
                render={({ field, fieldState, formState }) => {
                    return <TextField
                        type="text"
                        name="name"
                        label="Name"
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
                    minLength: {
                        value: 3,
                        message: 'Description length must be at least 3'
                    },
                    maxLength: {
                        value: 100,
                        message: 'Description length must be at most 100'
                    }
                }}
                render={({ field, fieldState, formState }) => {
                    return <TextField
                        type=""
                        multiline
                        name="description"
                        label="Description"
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
                name={'price'}
                rules={{
                    required: {
                        value: true,
                        message: 'Price is required'
                    },
                    min: {
                        value: 0,
                        message: 'Price must not be negative'
                    }
                }}
                render={({ field, fieldState, formState }) => {
                    return <TextField
                        type="number"
                        multiline
                        name="price"
                        label="Price"
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
                name={'unitsInStock'}
                rules={{
                    required: {
                        value: true,
                        message: 'Stock is required'
                    },
                    min: {
                        value: 1,
                        message: 'Stock must be positive'
                    }
                }}
                render={({ field, fieldState, formState }) => {
                    return <TextField
                        type="number"
                        multiline
                        name="unitsInStock"
                        label="Stock"
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
                name={'imageUrl'}
                rules={{
                    pattern: {
                        value: /https?:\/\/(www\.)?[-a-zA-Z0-9@:%._+~#=]{1,256}\.[a-zA-Z0-9()]{1,6}\b([-a-zA-Z0-9()@:%_+.~#?&//=]*)/,
                        message: 'Image URL must be valid'
                    }
                }}
                render={({ field, fieldState, formState }) => {
                    return <TextField
                        // type="url"
                        multiline
                        name="imageUrl"
                        label="Image URL"
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
        {/* <Grid item xs={12}>
            <Controller
                control={form.control}
                name={'isListed'}
                render={({ field, fieldState, formState }) => {
                    return <FormControlLabel
                        control={
                            <Checkbox
                                checked={field.value}
                                onChange={(e) => {
                                    field.onChange(e.target.checked);
                                }}
                                name="isListed"
                            />
                        }
                        label={<Typography>List Product</Typography>}

                        sx={(theme) => ({
                            input: {
                                color: theme.palette.primary.contrastText
                            }
                        })}
                    />
                }}
            />
        </Grid> */}
        {/* <Grid item xs={12}>
        <Controller
          control={form.control}
          name={"categoryId"}
          render={({ field, fieldState, formState }) => {
            return (
              <FormControl fullWidth>
                <InputLabel id="category-select-label">Category</InputLabel>
                <Select
                  labelId="category-select-label"
                  id="category-select"
                  label="Category"
                
                  value={field.value}
                  onChange={(e) => {
                    field.onChange(e.target.value);
                  }}
                  onBlur={field.onBlur}
                  ref={field.ref}
                  error={!!fieldState.error}
                  // helperText={fieldState.error?.message}
                >
                  {APP_CATEGORIES.map(c => (
                    <MenuItem key={c.id} value={c.id}>{c.name}</MenuItem>
                  ))}
                </Select>
              </FormControl>
            );
          }}
        />
      </Grid> */}
    </>);
};