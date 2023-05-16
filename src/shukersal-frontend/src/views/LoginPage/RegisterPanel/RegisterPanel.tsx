import React, { SyntheticEvent, useCallback, useEffect } from 'react';
import { FormProvider, useForm } from 'react-hook-form';
import { useMatch, useNavigate } from 'react-router-dom';

import { Box, FormControl, Grid, Link, Typography } from '@mui/material';
import { RegisterFormLayout } from './RegisterFormLayout';
import { RegisterFormFields } from '../../../types/formTypes';
import { useAuth } from '../../../hooks/useAuth';
import { LoadingButton } from '@mui/lab';
import { FlexSpacer } from '../../../components/FlexSpacer';

export const RegisterPanel = () => {
    const navigate = useNavigate();
    const form = useForm<RegisterFormFields>({
        defaultValues: {
            username: '',
            password: '',
            confirmPassword: ''
        }
    });
    const { isLoading, register, error } = useAuth();

    const handleRegister = useCallback((async () => {
        const isSuccess = await register(form.getValues());
        if (isSuccess) {
            navigate('/login', { relative: 'path' });
        }
    }), [navigate, form.getValues]);

    return (
        <FormProvider {...form}>
            <form onSubmit={(e) => {
                e.preventDefault();
                form.handleSubmit(handleRegister)();
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
                    <Grid
                        container
                        rowSpacing={2}
                        width="100%"
                    >
                        <RegisterFormLayout />
                    </Grid>
                    {error && <Typography variant="body1" color="error">
                        {error.message}
                    </Typography>}
                    <FlexSpacer minHeight={100} />
                    <LoadingButton
                        variant="contained"
                        color="secondary"
                        fullWidth
                        loading={isLoading}
                        type="submit"
                    // onClick={form.handleSubmit(handleLogin)}
                    //disabled={(() => {console.log('formState: ', form.formState); return !form.formState.isValid})()}
                    >Log In</LoadingButton>
                    <Box sx={{
                        width: '100%',
                        pt: 2,

                        display: 'flex',
                        justifyContent: 'center',
                        alignItems: 'center'
                    }}>
                        <Box sx={(theme) => ({
                            display: 'flex',
                            [theme.breakpoints.down('sm')]: {
                                flexDirection: 'column',
                                justifyContent: 'center',
                                alignItems: 'start'
                            },
                            [theme.breakpoints.up('sm')]: {
                                width: '100%',
                                flexDirection: 'row',
                                justifyContent: 'space-between',
                                alignItems: 'space-between'
                            }

                        })}>
                            <Typography><Link color="secondary" href="/login">
                                Login
                            </Link></Typography>

                            <Typography><Link color="secondary" href="/">
                                Continue as Guest
                            </Link></Typography>
                        </Box>
                    </Box>
                </FormControl >
            </form>
        </FormProvider >
    )
};