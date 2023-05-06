import { Card, Grid, CardContent, Typography, Box } from '@mui/material';

import { useCallback } from 'react';
import { useNavigate } from 'react-router-dom';

import { useMembers } from '../../hooks/useMembers';
import { AppPageScrollableContainer } from '../../components/AppPageScrollableContainer';
import { AppLoader } from '../../components/AppLoader/AppLoader';

export const BuyerPage = () => {
    const members = useMembers(); 
    return <AppPageScrollableContainer>
        <AppLoader/>
    </AppPageScrollableContainer>
}