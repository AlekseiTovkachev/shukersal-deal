import React, { useState, useCallback } from 'react';
import { Box, Paper } from '@mui/material';

import NavigateBeforeIcon from '@mui/icons-material/NavigateBefore';
import NavigateNextIcon from '@mui/icons-material/NavigateNext';

import { Carousel } from 'react-responsive-carousel';
import 'react-responsive-carousel/lib/styles/carousel.css';

interface AppCarouselProps {
    slides: JSX.Element[]
}

export const AppCarousel = ({ slides }: AppCarouselProps) => {
    return (
        <Paper sx={{
            width: '100%',
            overflow: 'hidden'
        }}>
            <Carousel
                showStatus={false}
                emulateTouch
                onClickItem={() => { console.log("[DEBUG] CLICK") }}
                showThumbs={false}
                showArrows
                //autoPlay
                infiniteLoop

            // renderArrowNext={(clickHandler, hasNext, label) => (
            //     <Box sx={{
            //         position: 'absolute',
            //         right: 0
            //     }} >
            //         <NavigateNextIcon />
            //     </Box>
            // )}
            >
                {slides}
            </Carousel>
        </Paper>
    );
}