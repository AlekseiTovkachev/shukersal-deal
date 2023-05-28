import { styled, Box } from '@mui/material';
export const AppMainPageContainer = styled(Box)(({ theme }) => {
    const bgColorA = theme.palette.background.default;
    //@ts-expect-error: Mui theme error
    const bgColorB = theme.palette.background.default2;
    //@ts-expect-error: Mui theme error
    const bgColorArtifacts = theme.palette.background.artifacts;
    // const bgLineWidth = '0px';
    // const bgShapeSize = '48px';
    return {
        position: 'fixed',
        width: '100%',
        height: '100%',
        display: 'flex',
        flexDirection: 'column',
        //justifyContent: 'center',
        alignItems: 'center',

        background: `
            conic-gradient(from -45deg at calc(100%/3) calc(100%/3), ${bgColorA} 90deg, #0000 0),
            conic-gradient(from -135deg at calc(100%/3) calc(2*100%/3), ${bgColorA} 90deg, ${bgColorB} 0 135deg, #0000 0),
            conic-gradient(from 135deg at calc(2*100%/3) calc(2*100%/3), ${bgColorA} 90deg, ${bgColorB} 0 135deg, #0000 0),
            conic-gradient(from 45deg at calc(2*100%/3) calc(100%/3), ${bgColorA} 90deg, ${bgColorB} 0 135deg, #0000 0,${bgColorA} 0 225deg,${bgColorB} 0);`,
        backgroundSize: `64px 64px;`,
        // background: `
        //     radial- gradient(calc(1.28 * ${ bgShapeSize } + ${ bgLineWidth } / 2) at left 50 % bottom calc(-.8 * ${ bgShapeSize }), ${ bgColorA } calc(100 % - ${ bgLineWidth }), ${ bgColorArtifacts } calc(101 % - ${ bgLineWidth }) 100 %,#0000 101 %) calc(2 * ${ bgShapeSize }) calc(-1 * calc(1.5 * ${ bgShapeSize } + ${ bgLineWidth })),
        // radial - gradient(calc(1.28 * ${ bgShapeSize } + ${ bgLineWidth } / 2) at left 50 % bottom calc(-.8 * ${ bgShapeSize }), ${ bgColorB } calc(100 % - ${ bgLineWidth }), ${ bgColorArtifacts } calc(101 % - ${ bgLineWidth }) 100 %,#0000 101 %) calc(-1 * ${ bgShapeSize }) calc(calc(1.5 * ${ bgShapeSize } + ${ bgLineWidth }) / -2),
        //     radial - gradient(calc(1.28 * ${ bgShapeSize } + ${ bgLineWidth } / 2) at left 50 % top    calc(-.8 * ${ bgShapeSize }), ${ bgColorB } calc(100 % - ${ bgLineWidth }), ${ bgColorArtifacts } calc(101 % - ${ bgLineWidth }) 100 %,#0000 101 %) 0 calc(1.5 * ${ bgShapeSize } + ${ bgLineWidth }),
        //         radial - gradient(calc(1.28 * ${ bgShapeSize } + ${ bgLineWidth } / 2) at left 50 % top    calc(-.8 * ${ bgShapeSize }), ${ bgColorA } calc(100 % - ${ bgLineWidth }), ${ bgColorArtifacts } calc(101 % - ${ bgLineWidth }) 100 %,#0000 101 %) ${ bgShapeSize } calc(calc(1.5 * ${ bgShapeSize } + ${ bgLineWidth }) / 2),
        //             linear - gradient(${ bgColorA } 50 %, ${ bgColorB } 0);,
        //backgroundSize: `calc(4 * ${ bgShapeSize }) calc(1.5 * ${ bgShapeSize } + ${ bgLineWidth }); `
    }
});