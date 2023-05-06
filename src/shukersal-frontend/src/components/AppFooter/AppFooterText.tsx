import { Typography, TypographyProps, Link, LinkProps } from '@mui/material';

interface AppFooterTextProps {
    children?: React.ReactNode,
    typographyProps?: TypographyProps,
    linkProps?: LinkProps
}

export const AppFooterText = ({ children, typographyProps = {}, linkProps = {} }: AppFooterTextProps) =>
    <Typography variant="body1" {...typographyProps}>
        <Link href="#" underline="none" color={(theme) => theme.palette.grey[700]} {...linkProps}>
            {children}
        </Link>
    </Typography >;