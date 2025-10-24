import { initRoot } from '../PopupBase.razor.js';
export function createDropDown(container, dotNet, closeOnOutsideClick, positionTarget) {
    const rootNode = initRoot();
    rootNode.appendChild(container);
    return new RyaClientDropDown(container, dotNet, closeOnOutsideClick, positionTarget);
}

export class RyaClientDropDown {
    constructor(container, dotNet, closeOnOutsideClick, positionTargetSelector) {
        this.rootContainer = container;
        this.positionTargetElement = document.querySelector(positionTargetSelector);
        this.dotNet = dotNet;

        if (closeOnOutsideClick) {
            this.onMouseDownHandler = (e) => this.onDocumentClick(this, e);
            document.addEventListener("mousedown", this.onMouseDownHandler);
        }

        this.x = 0;
        this.y = 0;

        if (this.positionTargetElement) {
            const clientRect = this.positionTargetElement.getBoundingClientRect();
            this.x = clientRect.x;
            this.y = clientRect.y + clientRect.height;
        }
    }

    setContent(content) {
        this.container = content.childNodes[0];
        this.container.style.left = this.x + "px";
        this.container.style.top = this.y + "px";
        this.rootContainer.classList.remove("d-none");
    }

    onDocumentClick(s, e) {
        if (s.container) {
            const rect = s.container.getBoundingClientRect();
            let outsideY = e.clientY < rect.top || e.clientY > rect.bottom;
            let outsideX = e.clientX < rect.left || e.clientX > rect.right;
            if (outsideX || outsideY) {
                s.dotNet.invokeMethodAsync('CloseDropDownAsync');
            }
        }
    }
}