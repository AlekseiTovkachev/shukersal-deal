import { styled, AppBar as MuiAppBar } from '@mui/material';

export const AppHeader = styled(MuiAppBar)(({ theme }) => ({
    position: 'relative',
    display: 'flex',
    flexDirection: 'row',
    alignItems: 'start',
    width: '100%',
    height: 50,
    boxSizing: 'border-box',
    padding: theme.spacing(1),

    // Should be styled in the theme
    //backgroundColor: '#ffffff',
    //boxShadow: '0px 2px 4px rgba(0, 0, 0, 0.2)',
}));