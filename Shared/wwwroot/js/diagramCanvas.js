window.initDiagramCanvas = (canvasId, dotNetRef) => {
    const canvas = document.getElementById(canvasId);
    if (!canvas) return;

    let isDragging = false;
    let animationFrameId = null;
    let lastMouseX = 0;
    let lastMouseY = 0;

    const getCanvasOffset = () => {
        const rect = canvas.getBoundingClientRect();
        return { x: rect.left, y: rect.top };
    };

    const handleMouseMove = (e) => {
        if (!isDragging) return;
        
        lastMouseX = e.clientX;
        lastMouseY = e.clientY;

        // Use requestAnimationFrame for smoother updates
        if (!animationFrameId) {
            animationFrameId = requestAnimationFrame(() => {
                const offset = getCanvasOffset();
                dotNetRef.invokeMethodAsync('OnCanvasMouseMove', 
                    lastMouseX - offset.x, 
                    lastMouseY - offset.y);
                animationFrameId = null;
            });
        }
    };

    const handleMouseUp = () => {
        isDragging = false;
        if (animationFrameId) {
            cancelAnimationFrame(animationFrameId);
            animationFrameId = null;
        }
        dotNetRef.invokeMethodAsync('OnCanvasMouseUp');
        document.removeEventListener('mousemove', handleMouseMove);
        document.removeEventListener('mouseup', handleMouseUp);
    };

    canvas.addEventListener('mousedown', (e) => {
        isDragging = true;
        const offset = getCanvasOffset();
        lastMouseX = e.clientX;
        lastMouseY = e.clientY;
        
        document.addEventListener('mousemove', handleMouseMove);
        document.addEventListener('mouseup', handleMouseUp);
    });
};