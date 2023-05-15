import { Box, Button } from '@mui/material';

interface AppHeaderButtonProps { 
    handleClick: () => void;
    children: React.ReactNode; 
}

export const AppHeaderButton = ({ handleClick, children }: AppHeaderButtonProps) => {
    return <Box
        //width="100%"
        //height="100%"
        display='flex'
        flexDirection='column'
        alignItems="center"
        justifyContent="center"
    >
        {/* <Typography variant='caption'>Welcome</Typography> */}
        <Button
            color='secondary'
            variant='contained'
            onClick={handleClick}
            size='small'
        >
            {children}
        </Button>
    </Box>
}