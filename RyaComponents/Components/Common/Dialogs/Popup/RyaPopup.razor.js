import { initRoot } from '../PopupBase.razor.js';

export function createPopup(container, dotNet, closeOnOutsideClick, allowDragging) {
    return new RyaClientPopup(container, dotNet, closeOnOutsideClick, allowDragging);
}

export class RyaClientPopup {
    constructor(container, dotNet, closeOnOutsideClick, allowDragging) {
        this.rootContainer = container;
        this.portal = null;
        this.container = null;
        this.dotNet = dotNet;
        this.allowDragging = allowDragging;
        this.x = 0;
        this.y = 0;

        if (closeOnOutsideClick) {
            this.documentOnMouseDownHandler = (e) => this.onDocumentClick(this, e);
            document.addEventListener("mousedown", this.documentOnMouseDownHandler);
        }

        const observer = new MutationObserver((mutations) => {
            const targetRemoved = mutations.some((mutation) => {
                const nodes = Array.from(mutation.removedNodes);
                return nodes.indexOf(this.portal) !== -1 || nodes.indexOf(this.rootContainer.getRootNode()) !== -1;
            });

            if ((targetRemoved || !this.rootContainer.parentNode) && this.container) {
                this.container.remove();
                this.observer && this.observer.disconnect();
                delete this.observer;
            }
        });

        observer.observe(document.body, { childList: true, subtree: true });
    }

    setContent(content) {
        this.portal = content.childNodes[0];
        this.container = this.portal.childNodes[0];
        this.content = this.container.childNodes[0];

        if (!this.container.style.left && !this.container.style.top) {
            this.container.style.left = this.x + "px";
            this.container.style.top = this.y + "px";
        }

        if (this.allowDragging) {
            this.containerOnMouseDownHandler = (e) => this.onStartDragging(this, e);
            this.container.addEventListener("mousedown", this.containerOnMouseDownHandler);
        }

        const rootNode = initRoot();
        rootNode.appendChild(this.container);

        this.portal.classList.remove("d-none");
    }

    onDocumentClick(s, e) {
        if (s.container) {
            const rect = s.container.getBoundingClientRect();
            let outsideY = e.clientY < rect.top || e.clientY > rect.bottom;
            let outsideX = e.clientX < rect.left || e.clientX > rect.right;
            if (outsideX || outsideY) {
                document.removeEventListener("mousedown", s.documentOnMouseDownHandler);
                s.dotNet.invokeMethodAsync('ClosePopupAsync');
            }
        }
    }

    onStartDragging(s, e) {
        e = e || window.event;
        e.preventDefault();
        s.x = e.clientX;
        s.y = e.clientY;

        document.onmouseup = (e) => s.onEndDragging(s, e);
        document.onmousemove = (e) => s.onDragging(s, e);
    }

    onEndDragging(s, e) {
        document.onmouseup = null;
        document.onmousemove = null;
    }

    onDragging(s, e) {
        e = e || window.event;
        e.preventDefault();
        s.container.style.top = (s.container.offsetTop - s.y + e.clientY) + "px";
        s.container.style.left = (s.container.offsetLeft - s.x + e.clientX) + "px";
        s.x = e.clientX;
        s.y = e.clientY;
    }
}