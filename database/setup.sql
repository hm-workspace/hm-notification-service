-- Database setup script for Notification Service
-- This script creates the Notifications table

-- Create the Notifications table
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Notifications' AND type = 'U')
BEGIN
    CREATE TABLE Notifications (
        Id INT PRIMARY KEY IDENTITY(1,1),
        Recipient NVARCHAR(255) NOT NULL,
        Channel NVARCHAR(50) NOT NULL,
        Subject NVARCHAR(500) NOT NULL,
        Message NVARCHAR(MAX) NOT NULL,
        Status NVARCHAR(50) NOT NULL,
        CreatedAt DATETIME2 NOT NULL,
        SentAt DATETIME2 NULL,
        FailureReason NVARCHAR(MAX) NOT NULL DEFAULT ''
    );

    -- Create indexes for commonly queried columns
    CREATE INDEX IX_Notifications_Status ON Notifications(Status);
    CREATE INDEX IX_Notifications_CreatedAt ON Notifications(CreatedAt DESC);
    CREATE INDEX IX_Notifications_Recipient ON Notifications(Recipient);
END
GO

-- Optional: Insert sample data for testing
-- Uncomment the following lines if you want to insert test data

/*
INSERT INTO Notifications (Recipient, Channel, Subject, Message, Status, CreatedAt, SentAt, FailureReason)
VALUES 
    ('john.doe@hm.local', 'Email', 'Appointment Reminder', 'Your appointment is scheduled for tomorrow at 10:00.', 'Sent', GETUTCDATE(), GETUTCDATE(), ''),
    ('jane.smith@hm.local', 'SMS', 'Test Results', 'Your test results are ready.', 'Queued', GETUTCDATE(), NULL, ''),
    ('bob.jones@hm.local', 'Email', 'Payment Receipt', 'Thank you for your payment.', 'Failed', GETUTCDATE(), NULL, 'Invalid email address');
*/
