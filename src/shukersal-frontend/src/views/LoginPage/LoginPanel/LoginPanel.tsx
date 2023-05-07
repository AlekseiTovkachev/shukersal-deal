import React, { useCallback, useEffect } from 'react';
import { FormProvider, useForm } from 'react-hook-form';
import { useMatch, useNavigate } from 'react-router-dom';

import { Box, FormControl, Grid, Link, Typography } from '@mui/material';
import { LoginFormLayout } from './LoginFormLayout';
import { LoginFormFields } from '../../../types/formTypes';
import { useAuth } from '../../../hooks/useAuth';
import { LoadingButton } from '@mui/lab';
import { FlexSpacer } from '../../../components/FlexSpacer';

export const LoginPanel = () => {
    const navigate = useNavigate();
    const form = useForm<LoginFormFields>({
        defaultValues: {
            username: '',
            password: '',
            rememberMe: false
        }
    });
    const { isLoading, login, error } = useAuth();

    const handleLogin = useCallback((async () => {
        const isSuccess = await login(form.getValues());
        if (isSuccess) {
            navigate('/', { relative: 'path' });
        }
    }), [navigate, form.getValues]);

    return (<FormProvider {...form}>
        <FormControl
            onSubmit={form.handleSubmit(handleLogin)}
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
                <LoginFormLayout />
            </Grid>
            {error && <Typography variant="body1" color="error">
                {error.message}
            </Typography>}
            <FlexSpacer />
            <LoadingButton
                variant="contained"
                color="secondary"
                fullWidth
                loading={isLoading}
                onClick={form.handleSubmit(handleLogin)}
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
                    <Link color="secondary" href="login/forgotpassword">
                        Forgot Password?
                    </Link>

                    <Link color="secondary" href="login/register">
                        Register
                    </Link>

                    <Link color="secondary" href="/">
                        Continue as Guest
                    </Link>
                </Box>
            </Box>
        </FormControl >
    </FormProvider>
    )
};