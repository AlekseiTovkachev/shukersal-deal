import React, { SyntheticEvent, useCallback, useEffect } from 'react';
import { FormProvider, useForm } from 'react-hook-form';
import { useMatch, useNavigate } from 'react-router-dom';

import { Box, FormControl, Grid, Link, Typography } from '@mui/material';
import { LoginFormLayout } from './LoginFormLayout';
import { LoginFormFields } from '../../../types/formTypes';
import { useAuth } from '../../../hooks/useAuth';
import { LoadingButton } from '@mui/lab';
import { FlexSpacer } from '../../../components/FlexSpacer';
import { errorToString } from '../../../util';

export const LoginPanel = () => {
    const navigate = useNavigate();
    const form = useForm<LoginFormFields>({
        defaultValues: {
            username: '',
            password: '',
            rememberMe: true
        }
    });
    const { isLoading, login, error } = useAuth();

    const handleLogin = useCallback((async () => {
        const isSuccess = await login(form.getValues());
        if (isSuccess) {
            navigate('/', { relative: 'path' });
        }
    }), [navigate, login, form]);

    return (
        <FormProvider {...form}>
            <form onSubmit={(e) => {
                e.preventDefault();
                form.handleSubmit(handleLogin)();
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
                        <LoginFormLayout />
                    </Grid>
                    {error && <Typography variant="body1" color="error">
                        {errorToString(error.message)}
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
                            {/* <Typography><Link color="secondary" href="/login/forgotpassword">
                                Forgot Password?
                            </Link></Typography> */}

                            <Typography><Link color="secondary" href="/login/register">
                                Register
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