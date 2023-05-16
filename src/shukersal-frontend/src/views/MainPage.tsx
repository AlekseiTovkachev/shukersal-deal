import { Outlet, useMatch, useResolvedPath } from 'react-router-dom';

import { Grid, Box, Typography } from '@mui/material';

import { AppHeader } from '../components/AppHeader/AppHeader';
import { AppMainPageContainer } from '../components/AppMainPageContainer/AppMainPageContainer';
import { AppFooter } from '../components/AppFooter/AppFooter';
import { AppFooterText } from '../components/AppFooter/AppFooterText';
import { AppFooterTitle } from '../components/AppFooter/AppFooterTitle';
import { AppPageScrollableContainer } from '../components/AppPageScrollableContainer';
import { FlexSpacer } from '../components/FlexSpacer';
import { AppLoginButton } from '../components/AppHeader/AppLoginButton';
import { AppLogoutButton } from '../components/AppHeader/AppLogoutButton';
import { AppLogo } from '../components/AppLogo/AppLogo';
import { useAuth } from '../hooks/useAuth';
import { AppSellerButton } from '../components/AppHeader/AppSellerButton';
import { AppBuyerButton } from '../components/AppHeader/AppBuyerButton';
//import { AppPagePaddingBox } from '../components/AppPagePaddingBox';

export const MainPage = () => {
    const loginPageMatch = useMatch({
        path: 'login',
        caseSensitive: false,
        end: false
    });
    const sellerPageMatch = useMatch({
        path: 'seller',
        caseSensitive: false,
        end: false
    });
    const fullscreen = !!loginPageMatch;
    const authData = useAuth();

    return <AppMainPageContainer>
        {fullscreen
            ? <Outlet />
            : <>
                <AppHeader>
                    <AppLogo />
                    {sellerPageMatch
                        ? <AppBuyerButton />
                        : <AppSellerButton />
                    }
                    <FlexSpacer />
                    {authData.isLoggedIn
                        ? <AppLogoutButton />
                        : <AppLoginButton />
                    }
                </AppHeader>
                <AppPageScrollableContainer>
                    <FlexSpacer>
                        <Box sx={(theme) => ({
                            p: 2,
                            boxSizing: 'border-box',
                            [theme.breakpoints.up('sm')]: {
                                px: 10,
                            },
                            [theme.breakpoints.up('md')]: {
                                px: 20,
                            },
                            [theme.breakpoints.up('lg')]: {
                                px: 25,
                            },
                            [theme.breakpoints.up('xl')]: {
                                px: 30,
                            },
                        })}><Outlet /></Box>
                    </FlexSpacer>
                    <AppFooter>
                        <Grid container spacing={3}>
                            <Grid item xs={6} display="flex" flexDirection="column">
                                <AppFooterTitle>Follow Us</AppFooterTitle>
                                {/* <AppFooterText>Facebook</AppFooterText>
                    <AppFooterText>Instagram</AppFooterText> */}
                                <AppFooterText linkProps={{ href: 'https://github.com/AlekseiTovkachev/shukersal-deal/' }}>Github</AppFooterText>
                            </Grid>
                            <Grid item xs={6} display="flex" flexDirection="column">
                                <AppFooterTitle>HELP</AppFooterTitle>
                                <AppFooterText>FAQ</AppFooterText>
                                <AppFooterText>Technical Support</AppFooterText>
                            </Grid>
                        </Grid>
                    </AppFooter>

                </AppPageScrollableContainer>
            </>}
    </AppMainPageContainer >
}