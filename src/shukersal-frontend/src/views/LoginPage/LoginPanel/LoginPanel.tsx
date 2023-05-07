import React, { useCallback, useEffect } from 'react';
import { FormProvider, useForm } from 'react-hook-form';
import { useMatch, useNavigate } from 'react-router-dom';

import { FormControl, Grid } from '@mui/material';
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
        await login(form.getValues());
        navigate('/', { relative: 'path' });
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
            <FlexSpacer />
            <LoadingButton
                        variant="contained"
                        color="secondary"
                        fullWidth
                        loading={isLoading}
                        onClick={form.handleSubmit(handleLogin)}
                    //disabled={(() => {console.log('formState: ', form.formState); return !form.formState.isValid})()}
                    >Submit</LoadingButton>
        </FormControl >
    </FormProvider>
    )
};