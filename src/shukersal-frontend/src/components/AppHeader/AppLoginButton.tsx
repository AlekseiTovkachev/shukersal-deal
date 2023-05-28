
import { Box, Button, Typography } from '@mui/material';
import { useCallback } from 'react';
import { useNavigate } from 'react-router-dom';
import { AppHeaderButton } from '../AppHeaderButton';

export const AppLoginButton = () => {
    const navigate = useNavigate();

    const handleClick = useCallback(() => {
        navigate('/login');
    }, [navigate]);

    return <AppHeaderButton handleClick={handleClick}>Login</AppHeaderButton>
};