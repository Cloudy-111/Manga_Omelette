export function DragAndInfinityCarousel() {
    const $menu = $('.menu')
    const $items = $('.menu--item')
    const prevBtn = $('#prevBtn');
    const nextBtn = $('#nextBtn');
    const container = $('.menu--item--container');

    let menuWidth = $menu.clientWidth
    let itemWidth = $items[0].clientWidth
    let wrapWidth = $items.length * itemWidth

    let scrollSpeed = 0
    let oldScrollY = 0
    let scrollY = 0
    let y = 0

    const adjustScrollPosition = () => {
        var carousel = document.querySelector('.menu--wrapper');
        const carouselLeft = carousel.getBoundingClientRect().left;
        const carouselWidth = carousel.offsetWidth;
        const carouselCenter = carouselLeft + carouselWidth / 2;

        let closestItem = null;
        let minDistance = Infinity;

        $items.forEach(item => {
            const itemRect = item.getBoundingClientRect();
            const itemCenter = (itemRect.left + itemRect.right) / 2;

            // Tính toán khoảng cách từ trung tâm của carousel đến trung tâm của item
            const distance = Math.abs(carouselCenter - itemCenter);

            // Chọn phần tử gần nhất
            if (distance < minDistance) {
                minDistance = distance;
                closestItem = item;
            }
        });

        if (closestItem) {
            // Cuộn đến phần tử gần nhất với trung tâm
            carousel.scrollTo({
                left: carousel.scrollLeft + closestItem.getBoundingClientRect().left - carouselLeft - carouselWidth / 2 + closestItem.offsetWidth / 2,
                behavior: 'smooth'
            });
        }
    }

    dispose(0)

    let touchStart = 0
    let touchX = 0
    let isDragging = false
    const handleTouchStart = (e) => {
        touchStart = e.clientX || e.touches[0].clientX
        isDragging = true
    }
    const handleTouchMove = (e) => {
        if (!isDragging) return
        touchX = e.clientX || e.touches[0].clientX
        scrollY += (touchX - touchStart)
        touchStart = touchX
    }
    const handleTouchEnd = () => {
        isDragging = false;
        //adjustScrollPosition();
    }

    $menu.on('mousedown', handleTouchStart)
    $menu.on('mousemove', handleTouchMove)
    $menu.on('mouseleave', handleTouchEnd)
    $menu.on('mouseup', handleTouchEnd)

    $menu.on('selectstart', () => { return false })

    let isDraggingLink = false;

    prevBtn.on('click', function () {
        scrollY += itemWidth;
        let redundant = scrollY % itemWidth;
        if (redundant != 0) {
            scrollY -= redundant;
        }
    })
    nextBtn.on('click', function () {
        scrollY -= itemWidth;
        let redundant = scrollY % itemWidth;
        if (redundant != 0) {
            scrollY -= redundant;
        }
    })

    window.addEventListener('resize', () => {
        menuWidth = $menu.clientWidth
        itemWidth = $items[0].clientWidth
        wrapWidth = $items.length * itemWidth
    })

    render()

    function lerp(v0, v1, t) {
        return v0 * (1 - t) + v1 * t
    }

    function dispose(scroll) {
        gsap.set($items, {
            x: (i) => {
                return i * itemWidth + scroll
            },
            modifiers: {
                x: (x, target) => {
                    const s = gsap.utils.wrap(-itemWidth, wrapWidth - itemWidth, parseInt(x))
                    return `${s}px`
                }
            }
        })
    }

    function render() {
        requestAnimationFrame(render)
        y = lerp(y, scrollY, .1)
        dispose(y)

        scrollSpeed = y - oldScrollY
        oldScrollY = y

        gsap.to($items, {
            rotate: scrollSpeed * .01,
        })
    }
}

