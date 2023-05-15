import { styled, AppBar as MuiAppBar, Stack } from '@mui/material';
import React from 'react';

const StyledAppBar = styled(MuiAppBar)(({ theme }) => ({
    position: 'relative',
    display: 'flex',
    flexDirection: 'row',
    alignItems: 'start',
    width: '100%',
    height: 75,
    boxSizing: 'border-box',
    padding: theme.spacing(1),
    // Should be styled in the theme
    //backgroundColor: '#ffffff',
    //boxShadow: '0px 2px 4px rgba(0, 0, 0, 0.2)',
}));

interface AppHeaderProps {
    children: React.ReactNode
}

export const AppHeader = ({ children }: AppHeaderProps) => {
    return <StyledAppBar>
        <Stack spacing={2} direction="row" sx={{width: '100%', height: '100%'}}>
            {children}
        </Stack>
    </StyledAppBar>
};