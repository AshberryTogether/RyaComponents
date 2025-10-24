export function createMenu(container, dotNet) {
    return new RyaClientMenu(container, dotNet);
}

export class RyaClientMenu {
    constructor(container, dotNet) {
        this.container = container;
        this.dotNet = dotNet;
        this.observer = null;

        this.trackWidths();
    }

    dispose() {
        if (this.observer) {
            this.observer.disconnect();
            this.observer = null;
        }
        this.container = null;
        this.dotNet = null;
    }

    getContainerWidth() {
        return this.container?.offsetWidth ?? 0;
    }

    getCollapsedWidth() {
        const hamburgerContainer = this.container.getElementsByClassName("hamburger-btn-container")[0];
        const menuItemsContainer = this.container.getElementsByClassName("rya-menu-items-container")[0];
        const menuItemsContainerHidden = menuItemsContainer.classList.contains("d-none");

        hamburgerContainer.classList.add("d-none");
        menuItemsContainer.classList.remove("d-none");
        menuItemsContainer.classList.add("d-flex");

        this.container.style.width = "fit-content";
        const width = this.container.offsetWidth;
        this.container.style.width = "";

        hamburgerContainer.classList.remove("d-none");
        if (menuItemsContainerHidden) {
            menuItemsContainer.classList.remove("d-flex");
            menuItemsContainer.classList.add("d-none");
        }

        return width;
    }

    trackWidths() {
        this.observer = new ResizeObserver(entries => {
            for (let entry of entries) {
                const width = entry.contentRect.width;
                this.dotNet.invokeMethodAsync("OnWidthChanged", width);
            }
        });

        this.observer.observe(this.container);
    }
}