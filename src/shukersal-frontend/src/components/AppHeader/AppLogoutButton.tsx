
import { Box, Button, Typography } from '@mui/material';
import { useCallback } from 'react';
import { useNavigate } from 'react-router-dom';
import { useAuth } from '../../hooks/useAuth';
import { devWarnNotLoggedIn } from '../../util';

export const AppLogoutButton = () => {
    const navigate = useNavigate();
    const authData = useAuth();

    const handleClick = useCallback(() => {
        authData.logout();
        navigate('/');
    }, []);
    if(!authData.isLoggedIn) {
        devWarnNotLoggedIn();
    }
    return <Box display='flex' flexDirection='column'>
        <Typography variant='caption'>Welcome, {authData.currentMemberData?.currentMember.username}</Typography>
        <Button
            color='secondary'
            variant='contained'
            onClick={handleClick}
        >
            Logout
        </Button>
    </Box>
};