
import { Box, Link, SxProps, Typography } from "@mui/material";

const companyLogoUrl = '/shukersal_logo.svg'
interface AppLogoProps {
    sx?: SxProps
}

export const AppLogo = ({ sx = {} }: AppLogoProps) => {

    return <Link href={'/'} sx={{
        display: 'block',
        width: 'auto',
        height: '100%',
        textDecotraion: 'none'
    }}>
        <Box
            component="img"
            src={companyLogoUrl}
            sx={{
                height: '100%',
                width: 'auto',
                background: 'rgba(0,0,0,0)',
                ...sx
            }}
        />
    </Link>
};