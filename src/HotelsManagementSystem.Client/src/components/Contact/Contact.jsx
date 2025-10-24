import styles from "./Contact.module.css";
import { useActionState } from "react";
import { contactFormAction } from "../../actions/contact/contact_form_action";

export default function Contact() {
  const [state, formAction, isPending] = useActionState(
    contactFormAction,
    null
  );

  return (
    <div className={styles.contact}>
      <div className={styles.contactHeader}>
        <h2>Contact Us</h2>
        <p>
          We'd love to hear from you. Send us a message and we'll respond as
          soon as possible.
        </p>
      </div>

      <form className={styles.contactForm} action={formAction}>
        {state?.message && (
          <div className={state.success ? styles.success : styles.error}>
            {state.message}
          </div>
        )}
        <div className={styles.formRow}>
          <div className={styles.formGroup}>
            <label htmlFor="firstName">First Name *</label>
            <input
              type="text"
              id="firstName"
              name="firstName"
              required
              disabled={isPending}
              defaultValue={state?.data?.firstName}
            />
            {state?.errors?.firstName && (
              <div className={styles.fieldError}>{state.errors.firstName}</div>
            )}
          </div>
          <div className={styles.formGroup}>
            <label htmlFor="lastName">Last Name *</label>
            <input
              type="text"
              id="lastName"
              name="lastName"
              required
              disabled={isPending}
              defaultValue={state?.data?.lastName}
            />
            {state?.errors?.lastName && (
              <div className={styles.fieldError}>{state.errors.lastName}</div>
            )}
          </div>
        </div>

        <div className={styles.formRow}>
          <div className={styles.formGroup}>
            <label htmlFor="email">Email Address *</label>
            <input
              type="email"
              id="email"
              name="email"
              required
              disabled={isPending}
              defaultValue={state?.data?.email}
            />
            {state?.errors?.email && (
              <div className={styles.fieldError}>{state.errors.email}</div>
            )}
          </div>
          <div className={styles.formGroup}>
            <label htmlFor="phoneNumber">Phone Number *</label>
            <input
              type="tel"
              id="phoneNumber"
              name="phoneNumber"
              required
              disabled={isPending}
              defaultValue={state?.data?.phoneNumber}
            />
            {state?.errors?.phoneNumber && (
              <div className={styles.fieldError}>
                {state.errors.phoneNumber}
              </div>
            )}
          </div>
        </div>

        <div className={styles.formGroup}>
          <label htmlFor="message">Message *</label>
          <textarea
            id="message"
            name="message"
            required
            placeholder="Tell us how we can help you..."
            disabled={isPending}
            defaultValue={state?.data?.message}
          ></textarea>
          {state?.errors?.message && (
            <div className={styles.fieldError}>{state.errors.message}</div>
          )}
        </div>

        <button
          type="submit"
          className={styles.submitButton}
          disabled={isPending}
        >
          {isPending ? "Sending..." : "Send Message"}
        </button>
      </form>
    </div>
  );
}
