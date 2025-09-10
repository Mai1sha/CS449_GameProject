//TIP To <b>Run</b> code, press <shortcut actionId="Run"/> or
// click the <icon src="AllIcons.Actions.Execute"/> icon in the gutter.
import java.util.Scanner;
//import org.junit.jupiter.api.Test;
//import static org.junit.jupiter.api.Assertions.assertEquals;
public class Main {

    public static void main(String[] args) {
        System.out.println("We will calculate BMI");
        Scanner sc = new Scanner(System.in);
        //declare variables;
        String first_name;
        String last_name;
        double height;
        double weight;
        double bmi;

        // Get the user's name
        System.out.println("What is your first name? ");
        first_name = sc.nextLine();

        System.out.println("What is your last name?");
        last_name = sc.nextLine();

        // Get the user's height and weight
        System.out.println("what is your height(in meter)? ");
        height = sc.nextDouble();

        System.out.println("what is your weight(in kgs.)? ");
        weight = sc.nextDouble();

        BmiCalc calculator = new BmiCalc();
        bmi = calculator.calculateBMI(weight, height);

        System.out.println("Your BMI is " + bmi);
    }
}
//helper class
class BmiCalc {
    public double calculateBMI(double weightKg, double heightM) {
        return weightKg / (heightM * heightM);
    }
}
