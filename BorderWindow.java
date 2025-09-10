

import javax.swing.*; // for Swing classes
import java.awt.*;    // for BorderLayout class


public class BorderWindow extends JFrame
{
    //for window height and width
    private final int WINDOW_WIDTH = 400;
    private final int WINDOW_HEIGHT = 350;


    public BorderWindow()
    {
        // Set the title bar text.
        setTitle("Border Layout");

        // Set the size of the window.
        setSize(WINDOW_WIDTH, WINDOW_HEIGHT);

        // action for the close button.
        setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);

        // Add a BorderLayout manager to the content pane.
        setLayout(new BorderLayout());

        // buttons.
        JButton button1 = new JButton("Top Button");
        //JButton button2 = new JButton("Bottom Button");
        JButton button3 = new JButton("Right Button");
        JButton button4 = new JButton("Left Button");
        JButton button5 = new JButton("Center Button");

        // Add the buttons to the content pane.
        add(button1, BorderLayout.NORTH);
        //add(button2, BorderLayout.SOUTH);
        add(button3, BorderLayout.EAST);
        add(button4, BorderLayout.WEST);
        add(button5, BorderLayout.CENTER);

        JCheckBox checkBox = new JCheckBox("Enable your option");
        //southPanel.add(checkBox, BorderLayout.WEST);

        //Radio buttons
        JRadioButton R_button1 = new JRadioButton("Option A");
        JRadioButton R_button2 = new JRadioButton("Option B");

        ButtonGroup group = new ButtonGroup();
        group.add(R_button1);
        group.add(R_button2);

        JPanel radioPanel = new JPanel(new FlowLayout(FlowLayout.RIGHT));
        radioPanel.add(R_button1);
        radioPanel.add(R_button2);

        //BOTTOM container panel
        JPanel southPanel = new JPanel(new BorderLayout());

        southPanel.add(checkBox, BorderLayout.WEST);
        southPanel.add(radioPanel, BorderLayout.EAST);

        // Add the whole south panel to the frame
        add(southPanel, BorderLayout.SOUTH);


        // Display the window.
        setVisible(true);
    }

    //main method
    public static void main(String[] args)
    {
        new BorderWindow();
    }
}
