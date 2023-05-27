
import { Box, Button, Typography } from '@mui/material';
import { useCallback } from 'react';
import { useNavigate } from 'react-router-dom';
import { AppHeaderButton } from '../AppHeaderButton';

export const AppBuyerButton = () => {
    const navigate = useNavigate();

    const handleClick = useCallback(() => {
        navigate('/');
    }, [navigate]);
    return <AppHeaderButton handleClick={handleClick}>Switch to Buying</AppHeaderButton>
};