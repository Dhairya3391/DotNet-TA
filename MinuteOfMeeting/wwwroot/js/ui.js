/**
 * UI JavaScript for Minutes of Meeting Management System
 * Handles Bulma-specific interactions and UI enhancements
 */

document.addEventListener('DOMContentLoaded', function() {
    // Initialize all UI components
    initNavbar();
    initModals();
    initNotifications();
    initFormValidation();
    initDeleteModals();
    initTooltips();
    initLoadingStates();
});

/**
 * Navbar - Mobile Burger Menu Toggle
 */
function initNavbar() {
    // Get all "navbar-burger" elements
    const $navbarBurgers = Array.prototype.slice.call(document.querySelectorAll('.navbar-burger'), 0);

    // Check if there are any navbar burgers
    if ($navbarBurgers.length > 0) {
        // Add a click event on each of them
        $navbarBurgers.forEach(el => {
            el.addEventListener('click', () => {
                // Get the target from the "data-target" attribute
                const target = el.dataset.target;
                const $target = document.getElementById(target);

                // Toggle the "is-active" class on both the "navbar-burger" and the "navbar-menu"
                el.classList.toggle('is-active');
                $target.classList.toggle('is-active');
            });
        });
    }
}

/**
 * Modal Management
 */
function initModals() {
    // Functions to open and close a modal
    function openModal($el) {
        $el.classList.add('is-active');
        document.body.style.overflow = 'hidden';
    }

    function closeModal($el) {
        $el.classList.remove('is-active');
        document.body.style.overflow = '';
    }

    function closeAllModals() {
        (document.querySelectorAll('.modal') || []).forEach(($modal) => {
            closeModal($modal);
        });
    }

    // Add a click event on buttons to open a specific modal
    (document.querySelectorAll('.js-modal-trigger') || []).forEach(($trigger) => {
        const modal = $trigger.dataset.target;
        const $target = document.getElementById(modal);

        $trigger.addEventListener('click', () => {
            openModal($target);
        });
    });

    // Add a click event on various child elements to close the parent modal
    (document.querySelectorAll('.modal-background, .modal-close, .modal-card-head .delete, .modal-card-foot .button') || []).forEach(($close) => {
        const $target = $close.closest('.modal');

        $close.addEventListener('click', () => {
            closeModal($target);
        });
    });

    // Add a keyboard event to close all modals
    document.addEventListener('keydown', (event) => {
        if (event.key === 'Escape') {
            closeAllModals();
        }
    });
}

/**
 * Notification Management
 */
function initNotifications() {
    // Auto-hide notifications after 5 seconds
    const notifications = document.querySelectorAll('.notification');
    notifications.forEach(notification => {
        const deleteButton = notification.querySelector('.delete');

        if (deleteButton) {
            deleteButton.addEventListener('click', () => {
                notification.remove();
            });
        }

        // Auto-hide after 5 seconds
        setTimeout(() => {
            if (notification.parentNode) {
                notification.style.opacity = '0';
                notification.style.transform = 'translateY(-10px)';
                setTimeout(() => {
                    if (notification.parentNode) {
                        notification.remove();
                    }
                }, 300);
            }
        }, 5000);
    });
}

/**
 * Form Validation Enhancement
 */
function initFormValidation() {
    const forms = document.querySelectorAll('form');

    forms.forEach(form => {
        // Add real-time validation feedback
        const inputs = form.querySelectorAll('input, textarea, select');

        inputs.forEach(input => {
            input.addEventListener('blur', () => {
                validateField(input);
            });

            input.addEventListener('input', () => {
                if (input.classList.contains('is-danger') || input.classList.contains('is-success')) {
                    validateField(input);
                }
            });
        });

        // Handle form submission
        form.addEventListener('submit', (e) => {
            let isValid = true;

            inputs.forEach(input => {
                if (!validateField(input)) {
                    isValid = false;
                }
            });

            if (!isValid) {
                e.preventDefault();
                showNotification('Please correct the errors before submitting.', 'error');
            }
        });
    });
}

/**
 * Validate Individual Field
 */
function validateField(field) {
    const value = field.value.trim();
    const isRequired = field.hasAttribute('required');
    const type = field.type;
    const parent = field.closest('.field');
    const helpText = parent ? parent.querySelector('.help') : null;

    // Reset classes
    field.classList.remove('is-danger', 'is-success');
    if (helpText) {
        helpText.classList.remove('is-danger', 'is-success');
        helpText.textContent = '';
    }

    // Required field validation
    if (isRequired && !value) {
        field.classList.add('is-danger');
        if (helpText) {
            helpText.textContent = 'This field is required';
            helpText.classList.add('is-danger');
        }
        return false;
    }

    // Email validation
    if (type === 'email' && value) {
        const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
        if (!emailRegex.test(value)) {
            field.classList.add('is-danger');
            if (helpText) {
                helpText.textContent = 'Please enter a valid email address';
                helpText.classList.add('is-danger');
            }
            return false;
        }
    }

    // Password validation
    if (type === 'password' && value && value.length < 6) {
        field.classList.add('is-danger');
        if (helpText) {
            helpText.textContent = 'Password must be at least 6 characters';
            helpText.classList.add('is-danger');
        }
        return false;
    }

    // Success state
    if (value) {
        field.classList.add('is-success');
        if (helpText && !helpText.textContent) {
            helpText.textContent = 'Looks good!';
            helpText.classList.add('is-success');
        }
    }

    return true;
}

/**
 * Delete Confirmation Modals
 */
function initDeleteModals() {
    const deleteButtons = document.querySelectorAll('.js-modal-trigger[data-target="delete-modal"]');

    deleteButtons.forEach(button => {
        button.addEventListener('click', () => {
            const id = button.dataset.id;
            const deleteForm = document.querySelector('#delete-form');
            const deleteIdInput = document.querySelector('#delete-id');

            if (deleteForm && deleteIdInput) {
                deleteIdInput.value = id;

                // Set the form action based on the current page
                const currentPath = window.location.pathname;
                const controller = currentPath.split('/')[1] || 'Meeting';
                deleteForm.action = `/${controller}/Delete/${id}`;
            }
        });
    });
}

/**
 * Tooltips and Popovers
 */
function initTooltips() {
    const tooltipTriggers = document.querySelectorAll('[title], [data-tooltip]');

    tooltipTriggers.forEach(trigger => {
        trigger.addEventListener('mouseenter', (e) => {
            const text = e.target.getAttribute('title') || e.target.getAttribute('data-tooltip');
            if (text) {
                showTooltip(e.target, text);
            }
        });

        trigger.addEventListener('mouseleave', () => {
            hideTooltip();
        });
    });
}

/**
 * Show Tooltip
 */
function showTooltip(element, text) {
    // Remove existing tooltip
    hideTooltip();

    const tooltip = document.createElement('div');
    tooltip.className = 'tooltip-content';
    tooltip.textContent = text;
    tooltip.style.cssText = `
        position: absolute;
        background: #363636;
        color: white;
        padding: 0.5rem 0.75rem;
        border-radius: 4px;
        font-size: 0.875rem;
        z-index: 1000;
        pointer-events: none;
        white-space: nowrap;
    `;

    document.body.appendChild(tooltip);

    const rect = element.getBoundingClientRect();
    tooltip.style.top = `${rect.top - tooltip.offsetHeight - 10}px`;
    tooltip.style.left = `${rect.left + (rect.width / 2) - (tooltip.offsetWidth / 2)}px`;
}

/**
 * Hide Tooltip
 */
function hideTooltip() {
    const existingTooltip = document.querySelector('.tooltip-content');
    if (existingTooltip) {
        existingTooltip.remove();
    }
}

/**
 * Loading States
 */
function initLoadingStates() {
    // Show loading modal on form submissions
    const forms = document.querySelectorAll('form');

    forms.forEach(form => {
        form.addEventListener('submit', () => {
            if (!form.hasAttribute('data-no-loading')) {
                showModal('loading-modal');
            }
        });
    });
}

/**
 * Modal Helper Functions
 */
function showModal(modalId) {
    const modal = document.getElementById(modalId);
    if (modal) {
        modal.classList.add('is-active');
        document.body.style.overflow = 'hidden';
    }
}

function hideModal(modalId) {
    const modal = document.getElementById(modalId);
    if (modal) {
        modal.classList.remove('is-active');
        document.body.style.overflow = '';
    }
}

/**
 * Notification Helper Functions
 */
function showNotification(message, type = 'info') {
    const notification = document.createElement('div');
    notification.className = `notification is-${type} is-light fade-in`;

    const iconMap = {
        'success': 'fa-check-circle',
        'danger': 'fa-exclamation-triangle',
        'warning': 'fa-exclamation-triangle',
        'info': 'fa-info-circle'
    };

    notification.innerHTML = `
        <button class="delete"></button>
        <span class="icon">
            <i class="fas ${iconMap[type] || iconMap['info']}"></i>
        </span>
        <span>${message}</span>
    `;

    // Insert at the top of the main content area
    const mainContent = document.querySelector('main .container');
    if (mainContent) {
        mainContent.insertBefore(notification, mainContent.firstChild);
    }

    // Add event listener to close button
    const deleteButton = notification.querySelector('.delete');
    deleteButton.addEventListener('click', () => {
        notification.remove();
    });

    // Auto-hide after 5 seconds
    setTimeout(() => {
        if (notification.parentNode) {
            notification.style.opacity = '0';
            notification.style.transform = 'translateY(-10px)';
            setTimeout(() => {
                if (notification.parentNode) {
                    notification.remove();
                }
            }, 300);
        }
    }, 5000);
}

/**
 * AJAX Helper Functions
 */
function showLoading() {
    showModal('loading-modal');
}

function hideLoading() {
    hideModal('loading-modal');
}

function handleAjaxError(xhr, status, error) {
    hideLoading();
    const message = xhr.responseJSON?.message || 'An error occurred. Please try again.';
    showNotification(message, 'danger');
}

function handleAjaxSuccess(response) {
    hideLoading();
    if (response.message) {
        showNotification(response.message, 'success');
    }
    if (response.redirect) {
        setTimeout(() => {
            window.location.href = response.redirect;
        }, 1500);
    }
}

/**
 * Chart Helper Functions (for Dashboard)
 */
function createChart(canvasId, type, data, options = {}) {
    const canvas = document.getElementById(canvasId);
    if (!canvas) return null;

    const ctx = canvas.getContext('2d');

    const defaultOptions = {
        responsive: true,
        maintainAspectRatio: false,
        plugins: {
            legend: {
                position: 'bottom',
            }
        }
    };

    return new Chart(ctx, {
        type: type,
        data: data,
        options: { ...defaultOptions, ...options }
    });
}

/**
 * Utility Functions
 */
function formatDateTime(dateString) {
    const date = new Date(dateString);
    return date.toLocaleString();
}

function formatDate(dateString) {
    const date = new Date(dateString);
    return date.toLocaleDateString();
}

function debounce(func, wait) {
    let timeout;
    return function executedFunction(...args) {
        const later = () => {
            clearTimeout(timeout);
            func(...args);
        };
        clearTimeout(timeout);
        timeout = setTimeout(later, wait);
    };
}

// Global error handler
window.addEventListener('error', (event) => {
    console.error('JavaScript error:', event.error);
    showNotification('An unexpected error occurred. Please refresh the page.', 'danger');
});

// Export functions for use in other scripts
window.UIManager = {
    showModal,
    hideModal,
    showNotification,
    showLoading,
    hideLoading,
    createChart,
    formatDateTime,
    formatDate
};