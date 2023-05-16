
import { Box, Button, Typography } from '@mui/material';
import { useCallback } from 'react';
import { useNavigate } from 'react-router-dom';
import { AppHeaderButton } from '../AppHeaderButton';

export const AppSellerButton = () => {
    const navigate = useNavigate();

    const handleClick = useCallback(() => {
        navigate('/seller');
    }, []);
    return <AppHeaderButton handleClick={handleClick}>Switch to Selling</AppHeaderButton>
};