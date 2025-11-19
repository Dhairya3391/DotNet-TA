// ==========================================================================
// MOM System - Site JavaScript
// ==========================================================================

$(document).ready(function() {
    // Initialize tooltips
    initializeTooltips();

    // Initialize date pickers
    initializeDatePickers();

    // Auto-hide alerts after 5 seconds
    autoHideAlerts();

    // Initialize form validations
    initializeFormValidations();

    // Initialize confirm dialogs
    initializeConfirmDialogs();

    // Initialize loading states
    initializeLoadingStates();
});

// ==========================================================================
// Utility Functions
// ==========================================================================

function initializeTooltips() {
    var tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'));
    var tooltipList = tooltipTriggerList.map(function (tooltipTriggerEl) {
        return new bootstrap.Tooltip(tooltipTriggerEl);
    });
}

function initializeDatePickers() {
    // Initialize date pickers with common settings
    $('.date-picker').each(function() {
        $(this).attr({
            'autocomplete': 'off',
            'data-bs-toggle': 'tooltip',
            'title': 'Click to select date'
        });
    });

    // Set min date for meeting date fields to today
    $('.meeting-date').each(function() {
        var today = new Date().toISOString().split('T')[0];
        $(this).attr('min', today);
    });
}

function autoHideAlerts() {
    setTimeout(function() {
        $('.alert').fadeOut(500, function() {
            $(this).remove();
        });
    }, 5000);
}

function showLoading(button, originalText) {
    button.prop('disabled', true);
    button.html('<span class="loading-spinner"></span> Processing...');
    button.data('original-text', originalText);
}

function hideLoading(button) {
    button.prop('disabled', false);
    button.html(button.data('original-text'));
}

function initializeLoadingStates() {
    $('form').on('submit', function(e) {
        var submitButton = $(this).find('button[type="submit"]');
        if (submitButton.length > 0) {
            showLoading(submitButton, submitButton.text());
        }
    });
}

function initializeFormValidations() {
    // Add real-time validation feedback
    $('.form-control').on('input', function() {
        validateField($(this));
    });

    $('.form-control').on('blur', function() {
        validateField($(this));
    });
}

function validateField(field) {
    var value = field.val().trim();
    var isValid = true;
    var errorMessage = '';

    // Remove previous validation state
    field.removeClass('is-valid is-invalid');
    field.next('.invalid-feedback').remove();

    // Check if field is required and empty
    if (field.prop('required') && value === '') {
        isValid = false;
        errorMessage = 'This field is required.';
    }

    // Email validation
    if (field.attr('type') === 'email' && value !== '') {
        var emailPattern = /^[^\s*[\w-]+([\.-][\w-]+)*@([\w-]+\.)+[\w-]{2,}\s*$/;
        if (!emailPattern.test(value)) {
            isValid = false;
            errorMessage = 'Please enter a valid email address.';
        }
    }

    // Add validation feedback
    if (!isValid) {
        field.addClass('is-invalid');
        field.after('<div class="invalid-feedback">' + errorMessage + '</div>');
    } else {
        field.addClass('is-valid');
    }

    return isValid;
}

function initializeConfirmDialogs() {
    $('.btn-delete').on('click', function(e) {
        e.preventDefault();
        var form = $(this).closest('form');
        var message = $(this).data('confirm') || 'Are you sure you want to delete this item?';

        if (confirm(message)) {
            form.submit();
        }
    });

    $('.btn-cancel').on('click', function(e) {
        e.preventDefault();
        var message = $(this).data('confirm') || 'Are you sure you want to cancel?';

        if (confirm(message)) {
            var form = $(this).closest('form');
            form.append('<input type="hidden" name="confirm" value="true" />');
            form.submit();
        }
    });
}

// ==========================================================================
// Form Helper Functions
// ==========================================================================

function validateForm(form) {
    var isValid = true;
    var firstInvalidField = null;

    form.find('.form-control[required]').each(function() {
        if (!validateField($(this))) {
            isValid = false;
            if (firstInvalidField === null) {
                firstInvalidField = $(this);
            }
        }
    });

    if (!isValid && firstInvalidField) {
        firstInvalidField.focus();
    }

    return isValid;
}

function resetForm(form) {
    form[0].reset();
    form.find('.form-control').removeClass('is-valid is-invalid');
    form.find('.invalid-feedback').remove();
}

// ==========================================================================
// AJAX Helper Functions
// ==========================================================================

function ajaxCall(url, method, data, successCallback, errorCallback) {
    $.ajax({
        url: url,
        method: method,
        data: data,
        dataType: 'json',
        beforeSend: function() {
            showLoadingOverlay();
        },
        success: function(response) {
            hideLoadingOverlay();
            if (successCallback) successCallback(response);
        },
        error: function(xhr, status, error) {
            hideLoadingOverlay();
            var errorMessage = 'An error occurred. Please try again.';

            if (errorCallback) {
                errorCallback(xhr, status, error);
            } else {
                showAlert('error', errorMessage);
            }
        }
    });
}

function showLoadingOverlay() {
    if (!$('#loadingOverlay').length) {
        $('body').append('<div id="loadingOverlay" class="loading-overlay"><div class="loading-spinner"></div></div>');
    }
}

function hideLoadingOverlay() {
    $('#loadingOverlay').fadeOut(300, function() {
        $(this).remove();
    });
}

// ==========================================================================
// Alert Functions
// ==========================================================================

function showAlert(type, message, title) {
    var alertClass = '';
    var icon = '';

    switch (type) {
        case 'success':
            alertClass = 'alert-success';
            icon = 'bi-check-circle-fill';
            break;
        case 'error':
            alertClass = 'alert-danger';
            icon = 'bi-exclamation-triangle-fill';
            break;
        case 'warning':
            alertClass = 'alert-warning';
            icon = 'bi-exclamation-triangle-fill';
            break;
        case 'info':
            alertClass = 'alert-info';
            icon = 'bi-info-circle-fill';
            break;
        default:
            alertClass = 'alert-primary';
            icon = 'bi-info-circle';
    }

    var alertHtml = `
        <div class="alert ${alertClass} alert-dismissible fade show" role="alert">
            <i class="bi ${icon} me-2"></i>
            ${message}
            <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
        </div>
    `;

    $('.content-wrapper .container').prepend(alertHtml);

    // Auto-hide after 5 seconds
    setTimeout(function() {
        $('.alert').fadeOut(500, function() {
            $(this).remove();
        });
    }, 5000);
}

// ==========================================================================
// Table Helper Functions
// ==========================================================================

function exportTableToExcel(tableId, fileName) {
    var table = document.getElementById(tableId);
    if (!table) {
        showAlert('error', 'Table not found');
        return;
    }

    var rows = [];
    var headers = [];

    // Get headers
    $(table).find('thead th').each(function() {
        headers.push($(this).text().trim());
    });

    // Get data rows
    $(table).find('tbody tr').each(function() {
        var row = [];
        $(this).find('td').each(function() {
            row.push($(this).text().trim());
        });
        rows.push(row);
    });

    // Create CSV content
    var csvContent = headers.join(',') + '\n';
    rows.forEach(function(row) {
        csvContent += row.join(',') + '\n';
    });

    // Create download link
    var blob = new Blob([csvContent], { type: 'text/csv' });
    var url = window.URL.createObjectURL(blob);
    var a = document.createElement('a');
    a.href = url;
    a.download = fileName + '.csv';
    document.body.appendChild(a);
    a.click();
    document.body.removeChild(a);
    window.URL.revokeObjectURL(url);
}

// ==========================================================================
// Chart Helper Functions
// ==========================================================================

function createBarChart(canvasId, data, label, options) {
    var ctx = document.getElementById(canvasId);
    if (!ctx) return;

    var defaultOptions = {
        responsive: true,
        maintainAspectRatio: false,
        plugins: {
            legend: {
                display: true,
                position: 'top'
            }
        },
        scales: {
            y: {
                beginAtZero: true
            }
        }
    };

    var chartOptions = $.extend(true, defaultOptions, options);

    return new Chart(ctx, {
        type: 'bar',
        data: {
            labels: data.labels,
            datasets: [{
                label: label,
                data: data.values,
                backgroundColor: 'rgba(54, 162, 235, 0.2)',
                borderColor: 'rgba(54, 162, 235, 1)',
                borderWidth: 1
            }]
        },
        options: chartOptions
    });
}

function createPieChart(canvasId, data, options) {
    var ctx = document.getElementById(canvasId);
    if (!ctx) return;

    var defaultOptions = {
        responsive: true,
        maintainAspectRatio: false,
        plugins: {
            legend: {
                display: true,
                position: 'right'
            }
        }
    };

    var chartOptions = $.extend(true, defaultOptions, options);

    return new Chart(ctx, {
        type: 'pie',
        data: {
            labels: data.labels,
            datasets: [{
                data: data.values,
                backgroundColor: [
                    '#FF6384',
                    '#36A2EB',
                    '#FFCE56',
                    '#4BC0C0',
                    '#9966FF',
                    '#FF9F40'
                ]
            }]
        },
        options: chartOptions
    });
}

function createLineChart(canvasId, data, label, options) {
    var ctx = document.getElementById(canvasId);
    if (!ctx) return;

    var defaultOptions = {
        responsive: true,
        maintainAspectRatio: false,
        plugins: {
            legend: {
                display: true,
                position: 'top'
            }
        },
        scales: {
            y: {
                beginAtZero: true
            }
        }
    };

    var chartOptions = $.extend(true, defaultOptions, options);

    return new Chart(ctx, {
        type: 'line',
        data: {
            labels: data.labels,
            datasets: [{
                label: label,
                data: data.values,
                borderColor: 'rgba(54, 162, 235, 1)',
                backgroundColor: 'rgba(54, 162, 235, 0.2)',
                tension: 0.4,
                fill: true
            }]
        },
        options: chartOptions
    });
}

// ==========================================================================
// File Upload Helper Functions
// ==========================================================================

function handleFileUpload(input, allowedTypes, maxSizeMB) {
    var file = input.files[0];

    if (!file) {
        return null;
    }

    // Check file type
    var fileExtension = file.name.split('.').pop().toLowerCase();
    if (allowedTypes.indexOf(fileExtension) === -1) {
        showAlert('error', `Invalid file type. Allowed types: ${allowedTypes.join(', ')}`);
        input.val('');
        return null;
    }

    // Check file size
    var maxSizeBytes = maxSizeMB * 1024 * 1024;
    if (file.size > maxSizeBytes) {
        showAlert('error', `File size cannot exceed ${maxSizeMB}MB`);
        input.val('');
        return null;
    }

    return file;
}

// ==========================================================================
// Date Helper Functions
// ==========================================================================

function formatDateTime(dateString) {
    var date = new Date(dateString);
    return date.toLocaleDateString() + ' ' + date.toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' });
}

function formatDateForInput(date) {
    if (!date) return '';
    return new Date(date).toISOString().split('T')[0];
}

function getDaysAgo(dateString) {
    var date = new Date(dateString);
    var now = new Date();
    var diffTime = Math.abs(now - date);
    var diffDays = Math.ceil(diffTime / (1000 * 60 * 60 * 24));

    return diffDays;
}

// ==========================================================================
// Number Helper Functions
// ==========================================================================

function formatNumber(num) {
    return num.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ',');
}

function calculatePercentage(part, total) {
    if (total === 0) return 0;
    return Math.round((part / total) * 100);
}

// ==========================================================================
// String Helper Functions
// ==========================================================================

function escapeHtml(text) {
    var div = document.createElement('div');
    div.textContent = text;
    return div.innerHTML;
}

function truncateText(text, maxLength) {
    if (text.length <= maxLength) return text;
    return text.substring(0, maxLength) + '...';
}

// ==========================================================================
// Session Helper Functions
// ==========================================================================

function checkSessionTimeout() {
    // This would be implemented with actual session checking logic
    // For now, just return false
    return false;
}

function refreshSession() {
    // This would refresh the session timeout
    console.log('Session refreshed');
}
