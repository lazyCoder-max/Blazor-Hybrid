window.initDiagramCanvas = (canvasId, dotNetRef) => {
    const canvas = document.getElementById(canvasId);
    if (!canvas) return;

    let isDragging = false;
    let draggedElement = null;
    let startX = 0;
    let startY = 0;
    let initialLeft = 0;
    let initialTop = 0;

    const handleMouseMove = (e) => {
        if (!isDragging || !draggedElement) return;

        const deltaX = e.clientX - startX;
        const deltaY = e.clientY - startY;

        // Use transform for GPU-accelerated rendering
        const newX = initialLeft + deltaX;
        const newY = initialTop + deltaY;

        draggedElement.style.transform = `translate(${deltaX}px, ${deltaY}px)`;
    };

    const handleMouseUp = (e) => {
        if (!isDragging || !draggedElement) return;

        const deltaX = e.clientX - startX;
        const deltaY = e.clientY - startY;

        const finalX = initialLeft + deltaX;
        const finalY = initialTop + deltaY;

        // Reset transform
        draggedElement.style.transform = '';

        // Update Blazor only once at the end
        const nodeId = draggedElement.getAttribute('data-node-id');
        if (nodeId) {
            dotNetRef.invokeMethodAsync('OnNodeDragComplete', nodeId, finalX, finalY);
        }

        isDragging = false;
        draggedElement = null;

        document.removeEventListener('mousemove', handleMouseMove);
        document.removeEventListener('mouseup', handleMouseUp);
    };

    canvas.addEventListener('mousedown', (e) => {
        // Find the node wrapper (the div with position:absolute)
        let target = e.target;
        while (target && target !== canvas) {
            if (target.hasAttribute('data-node-id')) {
                draggedElement = target;
                break;
            }
            target = target.parentElement;
        }

        if (!draggedElement) return;

        isDragging = true;
        startX = e.clientX;
        startY = e.clientY;

        // Get current position
        const style = window.getComputedStyle(draggedElement);
        initialLeft = parseInt(style.left) || 0;
        initialTop = parseInt(style.top) || 0;

        document.addEventListener('mousemove', handleMouseMove);
        document.addEventListener('mouseup', handleMouseUp);

        e.preventDefault();
    });
};