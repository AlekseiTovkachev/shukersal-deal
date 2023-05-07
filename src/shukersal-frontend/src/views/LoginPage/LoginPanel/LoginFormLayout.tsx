import { useMemo } from 'react';

import { Controller, useFormContext } from 'react-hook-form';

import { Typography, Divider, TextField, Grid, Box, useTheme, useMediaQuery, FormControlLabel, Checkbox } from '@mui/material';
import { LoginFormFields } from '../../../types/formTypes';

import CheckBoxOutlineBlankIcon from '@mui/icons-material/CheckBoxOutlineBlank';
import CheckBoxIcon from '@mui/icons-material/CheckBox';

import './styles.css';

export const LoginFormLayout = () => {
    const theme = useTheme();
    const isScreenLessThanMedium = useMediaQuery(theme.breakpoints.down("md"));

    const form = useFormContext<LoginFormFields>();
    const formValues = form.getValues();

    return (<>
        <Grid item xs={12} sx={{ display: 'flex', pt: '0 !important', width: '100%', justifyContent: 'center' }}>
            <Box className="app-typewriter-container">
                <Typography
                    className="app-typewriter-text"
                    variant="h5"
                    color="primary.contrastText"
                >The best marketplace</Typography>
            </Box>
        </Grid>
        <Grid item xs={12}>
            <Controller
                control={form.control}
                name={'username'}
                rules={{
                    required: {
                        value: true,
                        message: 'Username is required'
                    }
                }}
                render={({ field, fieldState, formState }) => {
                    return <TextField
                        type="text"
                        name="username"
                        placeholder="Username"
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
                                color: theme.palette.primary.contrastText
                            }
                        })}
                    />
                }}
            />
        </Grid>
        <Grid item xs={12}>
            <Controller
                control={form.control}
                name={'password'}
                rules={{
                    required: {
                        value: true,
                        message: 'Password is required'
                    },
                    minLength: {
                        value: 6,
                        message: 'Password must be at least 6 characters long'
                    },
                    maxLength: {
                        value: 45,
                        message: 'Password cannot be longer than 45 characters'
                    },
                }}
                render={({ field, fieldState, formState }) => {
                    return <TextField
                        type="password"
                        name="password"
                        placeholder="Password"
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

                    />
                }}
            />
        </Grid>
        <Grid item xs={12}>
            <Controller
                control={form.control}
                name={'rememberMe'}
                render={({ field, fieldState, formState }) => {
                    return <FormControlLabel
                        control={
                            <Checkbox
                                checked={field.value}
                                onChange={(e) => {
                                    field.onChange(e.target.checked);
                                }}
                                name="rememberMe"

                                color='secondary'
                                icon={<CheckBoxOutlineBlankIcon color="secondary"/>}
                                checkedIcon={<CheckBoxIcon color="secondary"/>}
                            />
                        }
                        label={<Typography color="primary.contrastText">Remember Me</Typography>}
                    />
                }}
            />
        </Grid>
    </>);
};