
import { Box, Button, Typography } from '@mui/material';
import { useCallback } from 'react';
import { useNavigate } from 'react-router-dom';
import { useAuth } from '../../hooks/useAuth';
import { devWarnNotLoggedIn } from '../../util';
import { useIsMobile } from '../../hooks/mediaHooks';
import { AppHeaderButton } from '../AppHeaderButton';

export const AppLogoutButton = () => {
    const isMobile = useIsMobile();

    const navigate = useNavigate();
    const authData = useAuth();

    const handleClick = useCallback(() => {
        authData.logout();
        navigate('/');
    }, []);
    if (!authData.isLoggedIn) {
        devWarnNotLoggedIn();
    }
    return <Box display='flex' flexDirection='column' justifyContent="center">
        <Typography variant='caption'>{!isMobile && ('Welcome, ' + authData.currentMemberData?.currentMember.username)}</Typography>
        <AppHeaderButton handleClick={handleClick}>Logout</AppHeaderButton>
    </Box>
};